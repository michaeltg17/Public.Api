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

namespace IntegrationTests
{
    public class WebApplicationFactoryFixture : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public ITestOutputHelper TestOutputHelper { get; set; } = default!;

        readonly MsSqlContainer _sqlServerContainer = new MsSqlBuilder()
            //.WithName("IntegrationTestsSqlServer")
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();

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

        string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder(_sqlServerContainer.GetConnectionString())
            {
                InitialCatalog = "Database"
            };
            return builder.ConnectionString;
        }

        void DeployDacpac()
        {
            var services = new DacServices(_sqlServerContainer.GetConnectionString());
            var package = DacPackage.Load("Public.Database.dacpac");
            services.Deploy(package, "Database");
        }

        public async new Task DisposeAsync()
        {
            await _sqlServerContainer.DisposeAsync();
            await base.DisposeAsync();
        }
    }
}
