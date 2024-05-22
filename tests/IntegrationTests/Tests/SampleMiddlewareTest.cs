using FluentAssertions;
using Xunit;
using Serilog.Sinks.InMemory;
using Serilog.Sinks.InMemory.Assertions;

namespace IntegrationTests.Tests
{
    [Collection(nameof(ApiCollection))]
    public class SampleMiddlewareTest : Test
    {
        [Fact]
        public async Task X()
        {
            //When
            await ApiClient.ThrowInternalServerError();

            //Then
            InMemorySink.Instance
                .Should()
                .HaveMessage("{MiddlewareName} started.")
                .Appearing().Once()
                .WithProperty("MiddlewareName")
                .WithValue("SampleMiddleware");
        }
    }
}