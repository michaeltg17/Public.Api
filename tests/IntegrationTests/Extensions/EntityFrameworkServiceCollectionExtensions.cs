using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace IntegrationTests.Extensions
{
    public static class EntityFrameworkServiceCollectionExtensions
    {
        public static IServiceCollection RemoveDbContextOptions<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            var serviceDescriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            services.Remove(serviceDescriptor);
            return services;
        }
    }
}
