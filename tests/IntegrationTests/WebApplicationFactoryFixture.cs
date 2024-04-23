using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Serilog;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using CrossCutting;
using Persistence;
using IntegrationTests.Extensions;
using Serilog.Events;
using Xunit.DependencyInjection;
using Serilog.Extensions.Hosting;
using IntegrationTests.XUnit;

namespace IntegrationTests
{
    public class WebApplicationFactoryFixture(IMessageSink messageSink, ITestOutputHelperAccessor testOutputHelperAccessor) 
        : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public ITestOutputHelper TestOutputHelper { get; set; }
        ReloadableLogger Logger { get; set; }

        Database Database = default!;

        public async Task InitializeAsync()
        {
            Database = await Database.Initialize(messageSink);
            //CreateLogger();
        }

        //void CreateLogger()
        //{
        //    Logger = new LoggerConfiguration().CreateBootstrapLogger();
        //    Log.Logger = Logger;
        //}

        //public void ReloadSerilog()
        //{
        //    Logger = new LoggerConfiguration().CreateBootstrapLogger();
        //    Log.Logger = Logger;
        //    Logger.Reload(config => config.WriteTo.TestOutput(TestOutputHelper, outputTemplate: Program.SerilogConsoleTemplate));
        //}

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseSerilog((context, services, configuration) =>
            {
                Program.ApplyCommonSerilogConfiguration(context, services, configuration);
                configuration.WriteTo.TestOutput(() => TestOutputHelper, outputTemplate: Program.SerilogConsoleTemplate);
                if (TestOptions.EnableSqlLogging)
                {
                    #pragma warning disable CS0162 // Unreachable code detected
                    configuration.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information);
                    #pragma warning restore CS0162 // Unreachable code detected
                }
            });

            builder.ConfigureServices(services =>
            {
                services.Configure<Settings>(settings =>
                {
                    settings.SqlServerConnectionString = Database.ConnectionString;
                    settings.Url = "http://localhost";
                });

                
                if (TestOptions.EnableSqlLogging)
                {
                    #pragma warning disable CS0162 // Unreachable code detected
                    services.RemoveDbContextOptions<AppDbContext>();
                    services.AddDbContext<AppDbContext>(options => options.EnableSensitiveDataLogging());
                    #pragma warning restore CS0162 // Unreachable code detected
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