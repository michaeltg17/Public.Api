using FluentAssertions;
using Xunit;
using ApiClient.Extensions;
using Domain.Models;
using ApiClient.Exceptions;

namespace IntegrationTests.Tests
{
    [Collection(nameof(ApiCollection))]
    public class ApiClientTests: Test
    {
        [Fact]
        public async Task WhenInternalServerError_ApiExceptionIsThrownWithExpectedProblemDetails()
        {
            //When
            var response = await ApiClient.ThrowInternalServerError();

            //Then
            var expectedMessage =
                "{\r\n" +
                "  \"type\": \"https://tools.ietf.org/html/rfc9110#section-15.6.1\",\r\n" +
                "  \"title\": \"InternalServerError\",\r\n" +
                "  \"status\": 500,\r\n" +
                "  \"detail\": \"Internal server error. Please contact the API support.\",\r\n" +
                "  \"instance\": \"/Test/ThrowInternalServerError\"\r\n" +
                "}";

            var func = async () => await response.To<ImageGroup>();
            await func.Should().ThrowAsync<ApiException>().WithMessage(expectedMessage);
        }
    }
}
