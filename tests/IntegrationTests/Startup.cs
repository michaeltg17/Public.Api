using IntegrationTests.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit.DependencyInjection;

namespace IntegrationTests
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<BeforeAfterTest, BeforeAfterTestConfiguration>();
            services.AddSingleton<WebApplicationFactoryFixture>();
            services.AddConfiguration();
        }

        static IServiceCollection AddConfiguration(this IServiceCollection services)
        {
            var configuration = new Dictionary<string, string?>
            {
                {"KeepAliveDatabase", "true"},
                {"ShouldDeployDacpac", "false"},
                {"EnableSqlLogging", "false"}
            };

            var configurationRoot = new ConfigurationBuilder()
                .AddInMemoryCollection(configuration)
                .Build();

            services.AddSingleton<IConfiguration>(configurationRoot);
            services.AddOptions<TestSettings>().BindConfiguration("");
            services.AddSingleton<ITestSettings>(provider => provider.GetRequiredService<IOptions<TestSettings>>().Value);

            return services;
        }
    }
}
