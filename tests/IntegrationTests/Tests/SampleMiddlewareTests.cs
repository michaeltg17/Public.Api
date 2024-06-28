using FluentAssertions;
using Xunit;
using Serilog.Sinks.InMemory.Assertions;

namespace IntegrationTests.Tests
{
    [Collection(nameof(ApiCollection))]
    public class SampleMiddlewareTests : Test
    {
        [Fact]
        public async Task LogsExpectedMessages()
        {
            //When
            await ApiClient.GetOk();

            //Then
            ValidateMessage("{middlewareName} started.");
            ValidateMessage("{middlewareName} finished.");

            void ValidateMessage(string message)
            {
                WebApplicationFactoryFixture.InMemorySink
                    .Should()
                    .HaveMessage(message)
                    .Appearing().Once()
                    .WithProperty("middlewareName")
                    .WithValue("SampleMiddleware");
            }
        }
    }
}