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
            ValidateMessage("{MiddlewareName} started.");
            ValidateMessage("{MiddlewareName} finished.");

            void ValidateMessage(string message)
            {
                WebApplicationFactoryFixture.InMemorySink
                    .Should()
                    .HaveMessage(message)
                    .Appearing().Once()
                    .WithProperty("MiddlewareName")
                    .WithValue("SampleMiddleware");
            }
        }
    }
}