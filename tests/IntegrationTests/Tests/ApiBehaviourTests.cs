using FluentAssertions;
using Xunit;
using Common.Testing.Builders;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ApiClient.Extensions;

namespace IntegrationTests.Tests
{
    [Collection(nameof(ApiCollection))]
    public class ApiBehaviourTests : Test
    {
        [InlineData(nameof(ApiClient.TestController))]
        [InlineData(nameof(ApiClient.TestMinimalApi))]
        [Theory]
        public async Task WhenInternalServerError_ExpectedProblemDetails(string testEndpointsName)
        {
            //When
            var response = await ApiClient.GetTestEndpoints(testEndpointsName).ThrowInternalServerError();

            //Then
            var expected = new ProblemDetailsBuilder()
                .WithInternalServerError($"/{testEndpointsName}/ThrowInternalServerError")
                .Build();

            var responseAsString = (await response.Content.ReadAsStringAsync()).ToLowerInvariant();
            responseAsString.Should().NotContain("Sensitive data".ToLowerInvariant());
            responseAsString.Should().NotContain("Exception".ToLowerInvariant());
            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task WhenNonexistentRoute_ExpectedProblemDetails()
        {
            //When
            var response = await ApiClient.RequestUnexistingRoute();

            //Then
            var expected = new ProblemDetailsBuilder()
                .WithNotFound()
                .Build();

            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [InlineData(nameof(ApiClient.TestController))]
        [InlineData(nameof(ApiClient.TestMinimalApi), Skip = "Waiting for asp net core team answer")]
        [Theory]
        public async Task WhenBadRequest_ExpectedProblemDetails(string testEndpointsName)
        {
            //When
            var response = await ApiClient.GetTestEndpoints(testEndpointsName).Get("this has to be an int");//.Get(1);

            //Then
            var expected = new ProblemDetailsBuilder()
                .WithValidationException($"/{testEndpointsName}/Get/this%20has%20to%20be%20an%20int")
                .WithError("id", "The value 'this has to be an int' is not valid.")
                .Build();

            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
