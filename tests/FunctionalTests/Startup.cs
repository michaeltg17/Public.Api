using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace FunctionalTests
{
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureHostConfiguration(builder => builder
                .AddJsonFile("testsettings.json")
                .AddEnvironmentVariables(Settings.SectionOrPrefix));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions<Settings>()
                .BindConfiguration(Settings.SectionOrPrefix)
                .ValidateOnStart()
                .ValidateDataAnnotations();

            services.AddSingleton<ISettings>(provider => provider.GetRequiredService<IOptions<Settings>>().Value);
        }
    }
}
