using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class WebApplicationFactoryFixture : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public ITestOutputHelper TestOutputHelper { get; set; } = default!;

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseSerilog((context, services, configuration) =>
            {
                Program.ApplySerilogConfiguration(context, services, configuration);
                configuration.WriteTo.TestOutput(TestOutputHelper);
            });

            return base.CreateHost(builder);
        }

        public new Task DisposeAsync()
        {
            return base.DisposeAsync().AsTask();
        }
    }
}
