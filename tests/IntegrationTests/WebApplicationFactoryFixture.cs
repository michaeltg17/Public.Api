using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Serilog;
using Xunit;
using Xunit.Abstractions;
using Testcontainers.MsSql;
using Microsoft.Extensions.DependencyInjection;
using CrossCutting;

namespace IntegrationTests
{
    public class WebApplicationFactoryFixture : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public ITestOutputHelper TestOutputHelper { get; set; } = default!;

        readonly MsSqlContainer _sqlServerContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();

        public async Task InitializeAsync()
        {
            await _sqlServerContainer.StartAsync();
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseSerilog((context, services, configuration) =>
            {
                Program.ApplySerilogConfiguration(context, services, configuration);
                configuration.WriteTo.TestOutput(TestOutputHelper);
            });

            builder.ConfigureServices(s => s.Configure<Settings>(c => c.SqlServerConnectionString = _sqlServerContainer.GetConnectionString()));

            return base.CreateHost(builder);
        }

        void DeployDacpac()
        {

        }

        public async new Task DisposeAsync()
        {
            await _sqlServerContainer.DisposeAsync();
            await base.DisposeAsync();
        }
    }
}
