using Application;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;
using Persistence;

namespace Api
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();
            AddMainDependencies(builder.Services);
            AddApplicationDependencies(builder.Services);

            var app = builder.Build();
            ConfigureAndRunApp(app);
        }

        static void AddApplicationDependencies(IServiceCollection services)
        {
            services.AddApplicationDependencies();
            services.AddPersistenceDependencies();
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

            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = 
                    HttpLoggingFields.RequestPath
                    | HttpLoggingFields.RequestMethod
                    | HttpLoggingFields.RequestBody
                    | HttpLoggingFields.ResponseStatusCode;
            });
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
            app.UseHttpLogging();

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