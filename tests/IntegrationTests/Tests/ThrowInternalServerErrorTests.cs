using Client;
using FluentAssertions;
using Xunit.Abstractions;
using Xunit;
using Common.Testing.Builders;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IntegrationTests.Tests
{
    [Collection("ApiCollection")]
    public class ThrowInternalServerErrorTests(ITestOutputHelper testOutputHelper, WebApplicationFactoryFixture factory) 
        : Test(testOutputHelper, factory)
    {
        [Fact]
        public async Task WhenInternalServerError_ExpectedProblemDetails()
        {
            //When
            var response = await apiClient.ThrowInternalServerError();

            //Then
            var expected = new ProblemDetailsBuilder()
                .WithInternalServerError("/Test/ThrowInternalServerError")
                .Build();

            var responseAsString = (await response.Content.ReadAsStringAsync()).ToLowerInvariant();
            responseAsString.Should().NotContain("Sensitive data".ToLowerInvariant());
            responseAsString.Should().NotContain("Exception".ToLowerInvariant());
            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
