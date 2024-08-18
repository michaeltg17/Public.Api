using ChinhDo.Transactions;
using CrossCutting.Settings;
using Core.Persistence;
using Microsoft.Extensions.FileProviders;

namespace Api.Storage
{
    internal static class DependencyConfigurator
    {
        internal static IServiceCollection AddObjectStorage(this IServiceCollection services)
        {
            services.AddScoped<IObjectStorage, ObjectStorage>(provider =>
            {
                var settings = provider.GetRequiredService<IApiSettings>();
                return new ObjectStorage(
                    settings.ImagesStoragePath,
                    settings.ImagesUrl,
                    provider.GetRequiredService<IFileManager>());
            });
            services.AddScoped<IFileManager>(p => new TxFileManager());

            return services;
        }

        internal static WebApplication UseObjectStorage(this WebApplication webApplication)
        {
            var apiSettings = webApplication.Services.GetRequiredService<IApiSettings>();
            webApplication.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = apiSettings.ImagesRequestPath,
                FileProvider = new PhysicalFileProvider(apiSettings.ImagesStoragePath)
            });

            return webApplication;
        }
    }
}
