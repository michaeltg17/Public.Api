using FluentAssertions;
using Xunit;
using Serilog.Sinks.InMemory.Assertions;
using Serilog.Events;

namespace IntegrationTests.Tests.Api.Filters
{
    [Collection(nameof(ApiCollection))]
    public class SampleFilterTests : Test
    {
        [Fact]
        public async Task LogsExpectedMessages()
        {
            //When
            await ApiClient.TestControllerApi.GetOk();

            //Then
            ValidateMessage("{filterName} started on {actionName}.");
            ValidateMessage("{filterName} finished on {actionName}.");

            void ValidateMessage(string message)
            {
                WebApplicationFactoryFixture.InMemorySink
                    .Should()
                    .HaveMessage(message)
                    .Appearing().Once()
                    .WithLevel(LogEventLevel.Information)
                    .WithProperty("filterName")
                    .WithValue("SampleFilter")
                    .And
                    .WithProperty("actionName")
                    .WithValue("Api.Controllers.TestController.GetOk (Api)");
            }
        }
    }
}