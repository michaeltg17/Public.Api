using Microsoft.Extensions.DependencyInjection;
using Application.Services;

namespace Application
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddScoped<ImageService>();
            services.AddScoped<TestService>();

            return services;
        }
    }
}
