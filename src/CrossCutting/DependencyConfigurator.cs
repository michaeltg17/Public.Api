using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CrossCutting
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection AddCrossCuttingDependencies(this IServiceCollection services)
        {
            services
                .AddOptions<Settings>()
                .BindConfiguration(Settings.SectionOrPrefix)
                .ValidateOnStart();

            services.AddSingleton<IValidateOptions<Settings>, SettingsValidator>();

            services.AddSingleton<ISettings>(provider => provider.GetRequiredService<IOptions<Settings>>().Value);

            return services;
        }
    }
}
