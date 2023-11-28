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
            AddSerilog(builder);
            AddMainDependencies(builder.Services);
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

        static void AddMainDependencies(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(c => c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Public API",
                    Description = "A showcase API"
                });
            });
        }

        static void ConfigureAndRunApp(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.MapControllers();

            AddObjectStorageFeature(app);

            app.Run();
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