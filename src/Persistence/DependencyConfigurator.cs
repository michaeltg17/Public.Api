using Microsoft.Extensions.DependencyInjection;

namespace Persistance.EntityFrameworkCore
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection AddPersistanceEntityFrameworkCoreDependencies(this IServiceCollection services)
        {
            services.AddDbContext<DbContext>();

            return services;
        }
    }
}
