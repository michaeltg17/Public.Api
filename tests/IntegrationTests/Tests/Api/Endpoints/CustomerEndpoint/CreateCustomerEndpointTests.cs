using ApiClient.Extensions;
using Application.Models.Requests;
using AutoFixture;
using FluentAssertions;
using System.Net;
using Xunit;

namespace IntegrationTests.Tests.Api.Endpoints.CustomerEndpoint
{
    [Collection(nameof(ApiCollection))]
    public class CreateCustomerEndpointTests : Test
    {
        [Fact]
        public async Task CustomerIsCreated()
        {
            //Given
            var request = new Fixture().Create<CreateCustomerRequest>();

            //When
            var response = await ApiClient.MinimalApi.CreateCustomer(request);
            var id = await response.To<long>();

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            id.Should().Be(1);
        }
    }
}
