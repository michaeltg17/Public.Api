﻿using Api;
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

namespace IntegrationTests
{
    public class WebApplicationFactoryFixture(IMessageSink messageSink) : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public ITestOutputHelper TestOutputHelper { get; set; } = default!;
        Database Database { get; set; } = default!;

        public async Task InitializeAsync()
        {
            Database = await Database.Initialize(messageSink);
        }

        /// <summary>
        /// To be called at the end of each test so that log from previous test doesn't get mixed with the next one.
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
                Program.ApplyCommonSerilogConfiguration(context, services, configuration);
                configuration.WriteTo.Map(
                    _ => TestOutputHelper,
                    (_, writeTo) => writeTo.TestOutput(TestOutputHelper),
                    sinkMapCountLimit: 1);

                if (TestOptions.EnableSqlLogging)
                {
                    #pragma warning disable CS0162 // Unreachable code detected
                    configuration.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information);
                    #pragma warning restore CS0162 // Unreachable code detected
                }
            });

            builder.ConfigureServices((context, services) =>
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