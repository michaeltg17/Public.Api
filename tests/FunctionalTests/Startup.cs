using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using FunctionalTests.Settings;

namespace FunctionalTests
{
    public static class Startup
    {
        public static void ConfigureHost(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureHostConfiguration(builder => builder
                .AddJsonFile("Settings/testsettings.json")
                .AddEnvironmentVariables());
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions<TestSettings>()
                .BindConfiguration("")
                .ValidateOnStart()
                .ValidateDataAnnotations();

            services.AddSingleton<ITestSettings>(provider => provider.GetRequiredService<IOptions<TestSettings>>().Value);
        }
    }
}
