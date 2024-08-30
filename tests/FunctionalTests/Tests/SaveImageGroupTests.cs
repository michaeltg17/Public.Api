using ApiClient.Extensions;
using Core.Testing.Models;
using FluentAssertions;
using System.Net;
using Xunit;

namespace FunctionalTests.Tests
{
    public class SaveImageGroupTests : Test
    {
        [InlineData(nameof(ApiClient.ControllerApi))]
        [InlineData(nameof(ApiClient.MinimalApi))]
        [Theory]
        public async Task GivenImage_WhenSaveImageGroup_IsSaved(string apiEndpointsName)
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";

            //When
            var response = await ApiClient.GetApiEndpoints(apiEndpointsName).SaveImageGroup(imagePath);
            var imageGroup = await response.To<ImageGroup>();

            //Then
            var imageGroup2 = await ApiClient.GetApiEndpoints(apiEndpointsName).GetImageGroup(imageGroup.Id).To<ImageGroup>();
            imageGroup.Should().BeEquivalentTo(imageGroup2);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
