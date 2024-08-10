using Microsoft.Extensions.DependencyInjection;
using Application.Services;
using ChinhDo.Transactions;
using Michael.Net.Persistence;
using Michael.Net.Persistence.AspNetCore;
using CrossCutting.Settings;

namespace Application
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddScoped<ImageService>();
            services.AddScoped<TestService>();

            services.AddScoped<IObjectStorage, AspNetCoreStorage>(provider =>
            {
                var settings = provider.GetRequiredService<IApiSettings>();
                return new AspNetCoreStorage(
                    settings.ImagesStoragePath,
                    settings.ImagesUrl,
                    provider.GetRequiredService<IFileManager>());
            });
            services.AddScoped<IFileManager>(p => new TxFileManager());

            return services;
        }
    }
}
