using FluentAssertions;
using Xunit;
using Core.Testing.Builders;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ApiClient.Extensions;
using Core.Testing.Extensions;
using Core.Testing;

namespace IntegrationTests.Tests.Api
{
    [Collection(nameof(ApiCollection))]
    public class ApiBehaviourTests : Test
    {
        [InlineData(nameof(ApiClient.TestControllerApi))]
        [InlineData(nameof(ApiClient.TestMinimalApi))]
        [Theory]
        public async Task WhenInternalServerError_ExpectedProblemDetails(string apiType)
        {
            //When
            var response = await ApiClient.GetTestEndpoints(apiType).ThrowInternalServerError();

            //Then
            var problemDetails = await response.To<ProblemDetails>();
            var traceId = problemDetails.GetTraceId();
            TraceIdValidator.IsValid(traceId).Should().BeTrue();

            var expected = new ProblemDetailsBuilder()
                .WithTraceId(traceId)
                .WithInternalServerError($"/{apiType}/ThrowInternalServerError")
                .Build();

            var responseAsString = (await response.Content.ReadAsStringAsync()).ToLowerInvariant();
            responseAsString.Should().NotContain("Sensitive data".ToLowerInvariant());
            responseAsString.Should().NotContain("Exception".ToLowerInvariant());
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task WhenNonexistentRoute_ExpectedProblemDetails()
        {
            //When
            var response = await ApiClient.RequestUnexistingRoute();

            //Then
            var problemDetails = await response.To<ProblemDetails>();
            var traceId = problemDetails.GetTraceId();
            TraceIdValidator.IsValid(traceId).Should().BeTrue();

            var expected = new ProblemDetailsBuilder()
                .WithTraceId(traceId)
                .WithNotFound()
                .Build();

            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [InlineData(nameof(ApiClient.TestControllerApi))]
        [InlineData(nameof(ApiClient.TestMinimalApi), Skip = "Waiting for asp net core team answer")]
        [Theory]
        public async Task WhenBadRequest_ExpectedProblemDetails(string apiType)
        {
            //When
            var parameter = "this has to be a long";
            var response = await ApiClient.GetTestEndpoints(apiType).Get(parameter);

            //Then
            var expected = new ProblemDetailsBuilder()
                .WithValidationException($"/{apiType}/Get/this%20has%20to%20be%20a%20long")
                .WithError("id", $"The value '{parameter}' is not valid.")
                .Build();

            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [InlineData(nameof(ApiClient.TestControllerApi))]
        [InlineData(nameof(ApiClient.TestMinimalApi), Skip = "Waiting for asp net core team answer")]
        [Theory]
        public async Task WhenComplexBadRequest_ExpectedProblemDetails(string apiType)
        {
            //When
            var response = await ApiClient.GetTestEndpoints(apiType).Post("a", 0, "b", null);

            //Then
            var expected = new ProblemDetailsBuilder()
                .WithValidationException($"/{apiType}/Post/a")
                .WithError("", $"A non-empty request body is required.")
                .WithError("date", $"The value 'b' is not valid.")
                .WithError("id", $"The value 'a' is not valid.")
                .WithError("request", $"The request field is required.")
                .Build();

            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
