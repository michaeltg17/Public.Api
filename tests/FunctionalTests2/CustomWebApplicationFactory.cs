using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Serilog;
using Xunit;

namespace FunctionalTests2
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public readonly TestOutputHelperProvider TestOutputHelperProvider = new();

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseSerilog((context, services, configuration) =>
            {
                Program.ApplySerilogConfiguration(context, services, configuration);
                configuration.WriteTo.TestOutput(TestOutputHelperProvider.Get());
            });

            return base.CreateHost(builder);
        }

        public Task DisposeAsync()
        {
            return base.DisposeAsync().AsTask();
        }
    }
}
