using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Serilog;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using CrossCutting;

namespace IntegrationTests
{
    public class WebApplicationFactoryFixture(IMessageSink messageSink) : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public ITestOutputHelper TestOutputHelper { get; set; } = default!;

        Database Database = default!;

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            Database = Database.Create(messageSink).GetAwaiter().GetResult();

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
