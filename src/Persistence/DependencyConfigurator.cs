using Microsoft.Extensions.DependencyInjection;

namespace Persistence
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection AddPersistanceDependencies(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();

            return services;
        }
    }
}
