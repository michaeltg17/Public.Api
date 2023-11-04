using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace FunctionalTests2
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return base.CreateHostBuilder().UseSerilog((context, services, configuration) =>
            {
                Program.ApplySerilogConfiguration(context, services, configuration);
                return configuration.WriteTo.TestOutput(output);
            });
        }
    }
}
