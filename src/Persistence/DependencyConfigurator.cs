using Michael.Net.Persistence.Factories;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using Dapper.Logging;
using Microsoft.Data.SqlClient;
using Application;
using Michael.Net.Persistence;

namespace Persistence
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<IDbConnectionFactory<IDbConnection>, SqlConnectionFactory>();

            services.AddDbConnectionFactory(
                p => new SqlConnection(p.GetRequiredService<ISettings>().SqlServerDapperConnectionString),
                config =>
                {
                    config.LogSensitiveData = true;
                    return config;
                });

            return services;
        }
    }
}
