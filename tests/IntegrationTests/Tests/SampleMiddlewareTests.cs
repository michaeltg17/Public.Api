using FluentAssertions;
using Xunit;
using Serilog.Sinks.InMemory.Assertions;
using Serilog.Events;

namespace IntegrationTests.Tests
{
    [Collection(nameof(ApiCollection))]
    public class SampleMiddlewareTests : Test
    {
        [Fact]
        public async Task LogsExpectedMessages()
        {
            //When
            await ApiClient.TestControllerApi.GetOk();

            //Then
            ValidateMessage("{middlewareName} started.");
            ValidateMessage("{middlewareName} finished.");

            void ValidateMessage(string message)
            {
                WebApplicationFactoryFixture.InMemorySink
                    .Should()
                    .HaveMessage(message)
                    .Appearing().Once()
                    .WithLevel(LogEventLevel.Information)
                    .WithProperty("middlewareName")
                    .WithValue("SampleMiddleware");
            }
        }
    }
}