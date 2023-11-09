using Microsoft.Extensions.DependencyInjection;
using Application.Services;
using Microsoft.AspNetCore.Hosting.Server.Features;
using ChinhDo.Transactions;
using Michael.Net.Persistence;
using Michael.Net.Persistence.AspNetCore;
using CrossCutting;

namespace Application
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddScoped<IImageService, ImageService>();

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
    }
}
