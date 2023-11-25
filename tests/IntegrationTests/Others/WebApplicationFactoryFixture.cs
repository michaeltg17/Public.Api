using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Serilog;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using CrossCutting;

namespace IntegrationTests.Others
{
    public class WebApplicationFactoryFixture : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public ITestOutputHelper TestOutputHelper { get; set; } = default!;

        Database Database = default!;

        public async Task InitializeAsync()
        {
            Database = await Database.Create();
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseSerilog((context, services, configuration) =>
            {
                Program.ApplySerilogConfiguration(context, services, configuration);
                configuration.WriteTo.TestOutput(TestOutputHelper);
            });

            builder.ConfigureServices(s => s.Configure<Settings>(s => 
            {
                s.SqlServerConnectionString = Database.ConnectionString;
                s.Url = "http://localhost";
            }));

            return base.CreateHost(builder);
        }

        public async new Task DisposeAsync()
        {
            await Database.DisposeAsync();
            await base.DisposeAsync();
        }
    }
}
