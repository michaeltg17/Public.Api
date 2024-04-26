using Microsoft.Extensions.DependencyInjection;
using Xunit.DependencyInjection;

namespace IntegrationTests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<BeforeAfterTest, BeforeAfterTestConfiguration>();
        }
    }
}
