using FluentAssertions;
using Xunit;
using Serilog.Sinks.InMemory.Assertions;

namespace IntegrationTests.Tests
{
    [Collection(nameof(ApiCollection))]
    public class SampleFilterTests : Test
    {
        [Fact]
        public async Task LogsExpectedMessages()
        {
            //When
            await ApiClient.GetOk();

            //Then
            ValidateMessage("{FilterName} started on {ActionName}.");
            ValidateMessage("{FilterName} finished on {ActionName}.");

            void ValidateMessage(string message)
            {
                WebApplicationFactoryFixture.InMemorySink
                    .Should()
                    .HaveMessage(message)
                    .Appearing().Once()
                    .WithProperty("FilterName")
                    .WithValue("SampleFilter")
                    .And
                    .WithProperty("ActionName")
                    .WithValue("Api.Controllers.TestController.GetOk (Api)");
            }
        }
    }
}