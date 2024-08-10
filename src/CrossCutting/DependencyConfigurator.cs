using CrossCutting.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CrossCutting
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection AddCrossCuttingDependencies(this IServiceCollection services)
        {
            services
                .AddOptions<ApiSettings>()
                .BindConfiguration(ApiSettings.SectionOrPrefix)
                .ValidateOnStart();

            services.AddSingleton<IValidateOptions<ApiSettings>, ApiSettingsValidator>();

            services.AddSingleton<IApiSettings>(provider => provider.GetRequiredService<IOptions<ApiSettings>>().Value);

            return services;
        }
    }
}
