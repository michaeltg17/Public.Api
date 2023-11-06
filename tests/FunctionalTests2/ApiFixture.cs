//using Api;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Serilog;
//using Serilog.Sinks.XUnit.Injectable;
//using Serilog.Sinks.XUnit.Injectable.Abstract;
//using Serilog.Sinks.XUnit.Injectable.Extensions;
//using Xunit;
//using Microsoft.Extensions.DependencyInjection;

//namespace FunctionalTests2
//{
//    public class ApiFixture : IAsyncLifetime
//    {
//        public WebApplicationFactory<Program> ApiFactory { get; set; } = default!;

//        public Task InitializeAsync()
//        {
//            ApiFactory = new CustomWebApplicationFactory<Program>();
//            ApiFactory = ApiFactory.WithWebHostBuilder(builder =>
//            {
//                // Instantiate the sink, with any configuration (like outputTemplate, formatProvider)
//                var injectableTestOutputSink = new InjectableTestOutputSink();

//                builder.ConfigureServices(services =>
//                {
//                    // Register the sink as a singleton
//                    services.AddSingleton<IInjectableTestOutputSink>(injectableTestOutputSink);
//                });

//                builder.UseSerilog((_, loggerConfiguration) =>
//                {
//                    // Add the sink to the logger configuration
//                    loggerConfiguration.WriteTo.InjectableTestOutput(injectableTestOutputSink);
//                });
//            });

//            return Task.CompletedTask;
//        }

//        public async Task DisposeAsync()
//        {
//            await ApiFactory.DisposeAsync();
//        }
//    }
//}
