using Application;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;
using Persistence;
using Serilog;

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

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
                
            services.AddCors(options =>
            {
                options.AddPolicy
                (
                    name: "AllowOrigin",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
            });

            services.AddScoped(p => p
                .GetRequiredService<IServer>()
                .Features
                .GetRequiredFeature<IServerAddressesFeature>());
        }

        static void ConfigureAndRunApp(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseCors("AllowOrigin");
            app.MapControllers();

            var settings = app.Services.GetRequiredService<ISettings>();
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = settings.ImagesRequestPath,
                FileProvider = new PhysicalFileProvider(settings.ImagesPath)
            });

            app.Run();
        }
    }
}