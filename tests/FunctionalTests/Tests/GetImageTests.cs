using ApiClient.Extensions;
using Core.Testing.Builders;
using Core.Testing.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace FunctionalTests.Tests
{
    public class GetImageTests : Test
    {
        [InlineData(nameof(ApiClient.ControllerApi))]
        [InlineData(nameof(ApiClient.MinimalApi))]
        [Theory]
        public async Task GivenImageGroup_WhenGetImage_IsGot(string apiEndpointsName)
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            var imageGroup = await ApiClient.GetApiEndpoints(apiEndpointsName).SaveImageGroup(imagePath).To<ImageGroup>();

            //When
            var image = await ApiClient.GetApiEndpoints(apiEndpointsName).GetImage(imageGroup.Images.First().Id).To<Image>();

            //Then
            var uploadedImageBytes = File.ReadAllBytes(imagePath);
            var downloadedImageBytes = await ApiClient.HttpClient.GetByteArrayAsync(image!.Url);

            uploadedImageBytes.Should().BeEquivalentTo(downloadedImageBytes);
        }

        [InlineData(nameof(ApiClient.ControllerApi))]
        [InlineData(nameof(ApiClient.MinimalApi))]
        [Theory]
        public async Task GivenUnexistingImage_WhenGetImage_ExpectedProblemDetails(string apiEndpointsName)
        {
            //Given
            //When
            var response = await ApiClient.GetApiEndpoints(apiEndpointsName).GetImage(id: 600);

            //Then
            var expected = new ProblemDetailsBuilder()
                .WithNotFoundException("/Image/600", "Image", 600)
                .Build();

            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
