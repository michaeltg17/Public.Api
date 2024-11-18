using ApiClient.Extensions;
using Core.Testing.Models;
using Core.Testing.Validators;
using FluentAssertions;
using System.Net;
using Xunit;

namespace IntegrationTests.Tests.Api.Endpoints.ImageGroupEndpoint
{
    [Collection(nameof(ApiCollection))]
    public class GetImageGroupEndpointTests : Test
    {
        [InlineData(nameof(ApiClient.ControllerApi))]
        [InlineData(nameof(ApiClient.MinimalApi))]
        [Theory]
        public async Task GivenImageGroup_WhenSaveAndGetImageGroup_IsGot(string apiType)
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            var imageGroup = await ApiClient.GetApiEndpoints(apiType).SaveImageGroup(imagePath).To<ImageGroup>();

            //When
            var response = await ApiClient.GetApiEndpoints(apiType).GetImageGroup(imageGroup.Id);
            var imageGroup2 = await response.To<ImageGroup>();

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            imageGroup.Should().BeEquivalentTo(imageGroup2);
            imageGroup.Images.Should().HaveCount(3);
        }

        [InlineData(nameof(ApiClient.ControllerApi))]
        [InlineData(nameof(ApiClient.MinimalApi))]
        [Theory]
        public async Task WhenGetNonexistentImageGroup_ExpectedProblemDetails(string apiType)
        {
            //When
            const long id = 600;
            var response = await ApiClient.GetApiEndpoints(apiType).GetImageGroup(id);

            //Then
            await ProblemDetailsValidator.ValidateNotFoundException(response, apiType, "ImageGroup", id);
        }
    }
}
