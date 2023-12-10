using Application;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;
using Persistence;
using Serilog;
using CrossCutting;
using Microsoft.OpenApi.Models;

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
            AddMiddlewares(app);

            app.Run();
        }

        static void AddMiddlewares(WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
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