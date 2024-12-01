using Microsoft.Extensions.DependencyInjection;
using Application.Services;

namespace Application
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services
                .AddScoped<ExcelExportService>()
                .AddScoped<ImageService>()
                .AddScoped<TestService>()
                .AddScoped<CustomerService>();

            return services;
        }
    }
}
