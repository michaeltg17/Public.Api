using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Serilog;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using IntegrationTests.Extensions;
using Serilog.Events;
using Serilog.Sinks.InMemory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using WebStartup.Middleware;
using Microsoft.AspNetCore.HttpLogging;
using CrossCutting.Settings;
using IntegrationTests.Settings;
using IntegrationTests.Database;

namespace IntegrationTests
{
    internal class WebApplicationFactoryFixture(ITestSettings testSettings, DatabaseFactory databaseFactory) 
        : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public ITestOutputHelper TestOutputHelper { get; set; } = default!;
        public InMemorySink InMemorySink { get; set; } = default!;
        Database.Database Database { get; set; } = default!;

        public async Task InitializeAsync()
        {
            Database = await databaseFactory.Create();
        }

        /// <summary>
        /// To be called at the end of each test so that logs from previous test doesn't get mixed with the next one.
        /// </summary>
        public static void FlushLogger()
        {
            //Not the best but too hard to do it in another way.
            Thread.Sleep(10);
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseSerilog((context, services, configuration) =>
            {
                Api.Startup.ApplyCommonSerilogConfiguration(context, services, configuration);
                //Using Map sink to fix "Only first test is logged"
                configuration.WriteTo.Map(
                    _ => TestOutputHelper,
                    (_, writeTo) => writeTo.TestOutput(TestOutputHelper),
                    sinkMapCountLimit: 1);
                configuration.WriteTo.Map(
                    _ => InMemorySink,
                    (_, writeTo) => writeTo.Sink(InMemorySink),
                    sinkMapCountLimit: 1);

                if (testSettings.EnableSqlLogging)
                {
                    configuration.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information);
                }
            });

            builder.ConfigureServices(services =>
            {
                services.AddHttpLogging(options => 
                    options.LoggingFields = HttpLoggingFields.RequestBody | HttpLoggingFields.ResponseBody);
                services.AddTransient<IStartupFilter, TestStartupFilter>();

                services.Configure<ApiSettings>(apiSettings =>
                {
                    apiSettings.SqlServerConnectionString = Database.ConnectionString;
                    //apiSettings.Url = "http://localhost";
                });

                if (testSettings.EnableSqlLogging)
                {
                    services.RemoveDbContextOptions<AppDbContext>();
                    services.AddDbContext<AppDbContext>(options => options.EnableSensitiveDataLogging());
                }
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