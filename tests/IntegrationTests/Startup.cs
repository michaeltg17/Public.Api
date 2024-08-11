using IntegrationTests.Database;
using IntegrationTests.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddSingleton<DatabaseFactory>();
        }

        public static void ConfigureHost(IHostBuilder hostBuilder)
        {
            hostBuilder.AddConfiguration();
        }

        static IHostBuilder AddConfiguration(this IHostBuilder builder)
        {
            var testSettings = new Dictionary<string, string?>
            {
                {"KeepAliveDatabase", "true"},
                {"ShouldDeployDacpac", "false"},
                {"EnableSqlLogging", "false"}
            };

            builder.ConfigureHostConfiguration(builder => builder.AddInMemoryCollection(testSettings));

            builder.ConfigureServices(services =>
            {
                services.AddOptions<TestSettings>().BindConfiguration("");
                services.AddSingleton<ITestSettings>(provider => provider.GetRequiredService<IOptions<TestSettings>>().Value);
            });

            return builder;
        }
    }
}
