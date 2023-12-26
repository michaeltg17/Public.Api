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
            hostBuilder.ConfigureHostConfiguration(builder => builder.AddJsonFile("testsettings.json"));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions<Settings>()
                .BindConfiguration("FunctionalTests")
                .ValidateOnStart()
                .ValidateDataAnnotations();

            services.AddSingleton<ISettings>(provider => provider.GetRequiredService<IOptions<Settings>>().Value);
        }
    }
}
