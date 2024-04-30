using FluentAssertions;
using Xunit;
using Common.Testing.Builders;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ApiClient.Extensions;

namespace IntegrationTests.Tests
{
    [Collection(nameof(ApiCollection))]
    public class ThrowInternalServerErrorTests : Test
    {
        [Fact]
        public async Task WhenInternalServerError_ExpectedProblemDetails()
        {
            //When
            var response = await ApiClient.ThrowInternalServerError();

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
