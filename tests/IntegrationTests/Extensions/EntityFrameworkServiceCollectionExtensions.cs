using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace IntegrationTests.Extensions
{
    public static class EntityFrameworkServiceCollectionExtensions
    {
        /// <summary>
        /// Newer EF Core versions won't need this. With AddDbContext or ConfigureDbContext will be enough.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RemoveDbContextOptions<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            var serviceDescriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            services.Remove(serviceDescriptor);
            return services;
        }
    }
}
