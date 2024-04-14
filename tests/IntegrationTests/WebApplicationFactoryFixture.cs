using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Serilog;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using CrossCutting;
using Persistence;

namespace IntegrationTests
{
    public class WebApplicationFactoryFixture(IMessageSink messageSink) : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public ITestOutputHelper TestOutputHelper { get; set; } = default!;

        Database Database = default!;

        public async Task InitializeAsync()
        {
            Database = await Database.Create(messageSink);
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseSerilog((context, services, configuration) =>
            {
                Program.ApplySerilogConfiguration(context, services, configuration);
                configuration.WriteTo.TestOutput(TestOutputHelper);
            });

            builder.ConfigureServices(services =>
            {
                services.Configure<Settings>(settings =>
                {
                    settings.SqlServerConnectionString = Database.ConnectionString;
                    settings.Url = "http://localhost";
                });

                //services.AddDbContext<AppDbContext>(options => 
                //{
                //    //options.EnableSensitiveDataLogging().LogTo(message => TestOutputHelper.WriteLine(message), Microsoft.Extensions.Logging.LogLevel.Trace);
                //    //options.EnableSensitiveDataLogging();
                //});
            });

            return base.CreateHost(builder);
        }

        public async new Task DisposeAsync()
        {
            await Database.DisposeAsync();
            await base.DisposeAsync();
        }
    }
}
