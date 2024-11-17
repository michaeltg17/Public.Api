using FluentAssertions;
using Xunit;
using ApiClient.Extensions;
using Domain.Models;
using ApiClient.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Core.Testing.Validators;

namespace IntegrationTests.Tests.ApiClient
{
    [Collection(nameof(ApiCollection))]
    public class ApiClientTests : Test
    {
        [InlineData(nameof(ApiClient.TestControllerApi))]
        [InlineData(nameof(ApiClient.TestMinimalApi))]
        [Theory]
        public async Task WhenInternalServerError_ApiExceptionIsThrownWithExpectedProblemDetails(string apiType)
        {
            //When
            var response = await ApiClient.GetTestEndpoints(apiType).ThrowInternalServerError();

            //Then
            var problemDetails = await response.To<ProblemDetails>();
            var traceId = Validator.ValidateTraceId(problemDetails);

            var expectedMessage = $$"""
                {
                  "type": "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                  "title": "InternalServerError",
                  "status": 500,
                  "detail": "Internal server error. Please contact the API support.",
                  "instance": "/{{apiType}}/ThrowInternalServerError",
                  "traceId": "{{traceId}}"
                }
                """;

            var func = response.To<ImageGroup>;
            await func.Should().ThrowAsync<ApiException>().WithMessage(expectedMessage);
        }

        [InlineData(nameof(ApiClient.TestControllerApi))]
        [InlineData(nameof(ApiClient.TestMinimalApi))]
        [Theory]
        public async Task WhenNoContent_ApiClientExceptionIsThrown(string apiType)
        {
            //When
            var response = await ApiClient.GetTestEndpoints(apiType).GetOk();

            //Then
            var func = response.To<ProblemDetails>;
            await func.Should().ThrowAsync<ApiClientException>().WithMessage("Response content is null, empty or whitespace.");
        }
    }
}
