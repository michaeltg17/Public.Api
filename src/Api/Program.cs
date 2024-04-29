using Application;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;
using Persistence;
using Serilog;
using CrossCutting;
using Microsoft.OpenApi.Models;
using Api.Extensions;
using Microsoft.AspNetCore.Mvc;

[assembly: ApiController]
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
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                ApplyCommonSerilogConfiguration(context, services, configuration);
                configuration.WriteTo.Console();
            });
        }

        public static void ApplyCommonSerilogConfiguration(
            HostBuilderContext context, IServiceProvider services, LoggerConfiguration configuration)
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                //.ReadFrom.Services(services)
                .Enrich.FromLogContext();
        }

        static void AddMainDependencies(WebApplicationBuilder builder)
        {
            AddSerilog(builder);

            builder.Services
                .AddControllers()
                .AddJsonOptions(c => c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddProblemDetails();
            AddSwaggerIfDevelopment(builder);

            builder.Services.Configure<ApiBehaviorOptions>(apiBehaviorOptions =>
            {
                apiBehaviorOptions.InvalidModelStateResponseFactory = actionContext =>
                {
                    var problemDetails = new ValidationProblemDetails(actionContext.ModelState)
                    {
                        Type = apiBehaviorOptions.ClientErrorMapping[400].Link,
                        Title = "ValidationException",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please check the errors property for additional details.",
                        Instance = actionContext.HttpContext.Request.Path
                    };

                    return new BadRequestObjectResult(problemDetails);
                };
            });
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
            app.AddExceptionHandler();
            app.Run();
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