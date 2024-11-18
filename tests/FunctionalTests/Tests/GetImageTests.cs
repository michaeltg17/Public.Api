using ApiClient.Extensions;
using Core.Testing.Models;
using Core.Testing.Validators;
using FluentAssertions;
using Xunit;

namespace FunctionalTests.Tests
{
    public class GetImageTests : Test
    {
        [InlineData(nameof(ApiClient.ControllerApi))]
        [InlineData(nameof(ApiClient.MinimalApi))]
        [Theory]
        public async Task GivenImageGroup_WhenGetImage_IsGot(string apiType)
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            var imageGroup = await ApiClient.GetApiEndpoints(apiType).SaveImageGroup(imagePath).To<ImageGroup>();

            //When
            var image = await ApiClient.GetApiEndpoints(apiType).GetImage(imageGroup.Images.First().Id).To<Image>();

            //Then
            var uploadedImageBytes = File.ReadAllBytes(imagePath);
            var downloadedImageBytes = await ApiClient.HttpClient.GetByteArrayAsync(image!.Url);

            uploadedImageBytes.Should().BeEquivalentTo(downloadedImageBytes);
        }

        [InlineData(nameof(ApiClient.ControllerApi))]
        [InlineData(nameof(ApiClient.MinimalApi))]
        [Theory]
        public async Task GivenUnexistingImage_WhenGetImage_ExpectedProblemDetails(string apiType)
        {
            //When
            const long id = 600;
            var response = await ApiClient.GetApiEndpoints(apiType).GetImage(id: 600);

            //Then
            await ProblemDetailsValidator.ValidateNotFoundException(response, apiType, "Image", id);
        }
    }
}
