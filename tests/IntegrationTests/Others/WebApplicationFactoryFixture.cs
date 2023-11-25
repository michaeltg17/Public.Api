using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Serilog;
using Xunit;
using Xunit.Abstractions;
using Testcontainers.MsSql;
using Microsoft.Extensions.DependencyInjection;
using CrossCutting;
using Microsoft.SqlServer.Dac;
using Microsoft.Data.SqlClient;

namespace IntegrationTests.Others
{
    public class WebApplicationFactoryFixture : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public ITestOutputHelper TestOutputHelper { get; set; } = default!;

        public async Task InitializeAsync()
        {
            await _sqlServerContainer.StartAsync();
            DeployDacpac();
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseSerilog((context, services, configuration) =>
            {
                Program.ApplySerilogConfiguration(context, services, configuration);
                configuration.WriteTo.TestOutput(TestOutputHelper);
            });

            builder.ConfigureServices(s => s.Configure<Settings>(c => c.SqlServerConnectionString = GetConnectionString()));

            return base.CreateHost(builder);
        }

        public async new Task DisposeAsync()
        {
            await _sqlServerContainer.DisposeAsync();
            await base.DisposeAsync();
        }
    }
}
