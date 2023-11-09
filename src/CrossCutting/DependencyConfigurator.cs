using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CrossCutting
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection AddCrossCuttingDependencies(this IServiceCollection services)
        {
            const string section = "Application";

            services
                .AddOptions<Settings>()
                .BindConfiguration(section)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton<ISettings>(provider => provider.GetRequiredService<IOptions<Settings>>().Value);

            return services;
        }
    }
}
