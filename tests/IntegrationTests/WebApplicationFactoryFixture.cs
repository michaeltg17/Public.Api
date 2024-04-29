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
using Serilog.Extensions.Hosting;

namespace IntegrationTests
{
    public class WebApplicationFactoryFixture(IMessageSink messageSink) : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public ITestOutputHelper TestOutputHelper { get; set; } = default!;
        ReloadableLogger ReloadableLogger { get; set; } = default!;
        LoggerConfiguration LoggerConfiguration { get; set; } = default!;
        Database Database { get; set; } = default!;

        public async Task InitializeAsync()
        {
            Database = await Database.Initialize(messageSink);
        }

        public void FlushLogger()
        {
            ReloadableLogger.Reload(_ => GetLoggerConfiguration());
        }

        LoggerConfiguration GetLoggerConfiguration()
        {
            return new LoggerConfiguration()
                            .WriteTo.Map(
                                _ => TestOutputHelper,
                                (_, writeTo) => writeTo.TestOutput(TestOutputHelper),
                                sinkMapCountLimit: 1);
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            LoggerConfiguration = new LoggerConfiguration()
                .WriteTo.Map(
                    _ => TestOutputHelper,
                    (_, writeTo) => writeTo.TestOutput(TestOutputHelper),
                    sinkMapCountLimit: 1);
            ReloadableLogger = LoggerConfiguration.CreateBootstrapLogger();

            //builder.UseSerilog((context, services, configuration) =>
            //{
            //    Program.ApplyCommonSerilogConfiguration(context, services, configuration);
            //    configuration.WriteTo.Map(
            //        _ => TestOutputHelper,
            //        (_, writeTo) => writeTo.TestOutput(TestOutputHelper),
            //        sinkMapCountLimit: 1);

            //    if (TestOptions.EnableSqlLogging)
            //    {
            //        #pragma warning disable CS0162 // Unreachable code detected
            //        configuration.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information);
            //        #pragma warning restore CS0162 // Unreachable code detected
            //    }
            //});

            builder.ConfigureServices((context, services) =>
            {
                //Program.ApplyCommonSerilogConfiguration(context, services, loggerConfiguration);
                services.AddSerilog(ReloadableLogger);

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
                    //configuration.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information);
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