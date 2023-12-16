using Application;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;
using Persistence;
using Serilog;
using CrossCutting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics;
using Application.Exceptions;
using System.Net;
using Microsoft.AspNetCore.Http.Features;
using Common.Net;

namespace Api
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            AddMainDependencies(builder);
            AddApplicationDependencies(builder.Services);

            var app = builder.Build();
            ConfigureAndRunApp(app);
        }

        static void AddApplicationDependencies(IServiceCollection services)
        {
            services.AddCrossCuttingDependencies();
            services.AddApplicationDependencies();
            services.AddPersistanceDependencies();
        }

        static void AddSerilog(WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog(ApplySerilogConfiguration);
        }

        public static void ApplySerilogConfiguration(HostBuilderContext context, IServiceProvider services, LoggerConfiguration configuration)
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console();
        }

        static void AddMainDependencies(WebApplicationBuilder builder)
        {
            AddSerilog(builder);

            builder.Services
                .AddControllers()
                .AddJsonOptions(c => c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddProblemDetails();

            AddSwaggerIfDevelopment(builder);
        }

        static void AddSwaggerIfDevelopment(WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Public API",
                        Description = "A showcase API"
                    });
                });
            }
        }

        static void ConfigureAndRunApp(WebApplication app)
        {
            AddSwaggerIfDevelopment(app);
            app.UseAuthorization();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.MapControllers();
            AddObjectStorageFeature(app);
            AddExceptionHandler(app);
            app.Run();
        }

        static void AddExceptionHandler(WebApplication app)
        {
            app.UseExceptionHandler(config => config.Run(async context =>
            {
                context.Response.ContentType = "application/problem+json";
                var problemDetailsService = context.RequestServices.GetRequiredService<IProblemDetailsService>();

                var exceptionHandlerFeature = context.Features.GetRequiredFeature<IExceptionHandlerFeature>();
                var exception = exceptionHandlerFeature.Error;

                context.Response.StatusCode = exception switch
                {
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    ApiException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                var problem = new ProblemDetailsContext
                {
                    Exception = exception,
                    HttpContext = context,
                    ProblemDetails =
                    {
                        Title = exception.GetType().GetNameWithoutGenericArity(),
                        Detail = exception.Message,
                        Status = context.Response.StatusCode
                    }
                };

                //Don't know if useful or not
                //if (app.Environment.IsDevelopment())
                //{
                //    problem.ProblemDetails.Extensions.Add("Exception", exception.ToString());
                //}

                var done = await problemDetailsService.TryWriteAsync(problem);
                if (!done) await context.Response.WriteAsJsonAsync(problem.ProblemDetails);
            }));
        }

        static void AddSwaggerIfDevelopment(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }

        static void AddObjectStorageFeature(WebApplication app)
        {
            var settings = app.Services.GetRequiredService<ISettings>();
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = settings.ImagesRequestPath,
                FileProvider = new PhysicalFileProvider(settings.ImagesStoragePath)
            });
        }
    }
}