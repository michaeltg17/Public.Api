using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Application.Services;
using Microsoft.AspNetCore.Hosting.Server.Features;
using ChinhDo.Transactions;
using Michael.Net.Persistence;
using Michael.Net.Persistence.AspNetCore;

namespace Application
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            AddConfiguration(services);
            services.AddScoped<IImageService, ImageService>();

            //services.AddScoped<IObjectStorage, BlobStorage>(provider =>
            //{
            //    var settings = provider.GetRequiredService<ISettings>();
            //    return new BlobStorage(
            //        settings.BlobContainerConnectionString,
            //        settings.BlobContainerName);
            //});

            services.AddScoped<IObjectStorage, AspNetCoreStorage>(provider =>
            {
                var settings = provider.GetRequiredService<ISettings>();
                return new AspNetCoreStorage(
                    settings.ImagesPath,
                    settings.ImagesRequestPath,
                    provider.GetRequiredService<IServerAddressesFeature>(),
                    provider.GetRequiredService<IFileManager>());
            });
            services.AddScoped<IFileManager>(p => new TxFileManager());

            return services;
        }

        static void AddConfiguration(IServiceCollection services)
        {
            const string section = "Application";

            services
                .AddOptions<Settings>()
                .BindConfiguration(section)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton<ISettings>(provider => provider.GetRequiredService<IOptions<Settings>>().Value);
        }
    }
}
