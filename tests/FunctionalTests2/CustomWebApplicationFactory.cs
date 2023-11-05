using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Serilog;
using Xunit.Abstractions;

namespace FunctionalTests2
{
    class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        readonly ITestOutputHelper testOutputHelper;

        public CustomWebApplicationFactory(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseSerilog((context, services, configuration) =>
            {
                Program.ApplySerilogConfiguration(context, services, configuration);
                configuration.WriteTo.TestOutput(testOutputHelper);
            });

            return base.CreateHost(builder);
        }
    }
}
