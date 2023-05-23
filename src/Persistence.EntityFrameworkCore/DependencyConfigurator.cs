using Michael.Net.Persistance;
using Microsoft.Extensions.DependencyInjection;

namespace Persistance.EntityFrameworkCore
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection AddPersistanceEntityFrameworkCoreDependencies(this IServiceCollection services)
        {
            services.AddDbContext<DbContext>();
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
