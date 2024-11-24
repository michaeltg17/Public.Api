using ApiClient.Extensions;
using Application.Models.Requests;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
            var request = new CreateCustomerRequest()
            {

            };

            //When
            var response = await ApiClient.MinimalApi.CreateCustomer(request);
            var imageGroup = await response.To<ImageGroup>();

            //Then
            var imageGroup2 = await ApiClient.GetApiEndpoints(apiType).GetImageGroup(imageGroup.Id).To<ImageGroup>();
            imageGroup.Should().BeEquivalentTo(imageGroup2);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
