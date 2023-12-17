using Client;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Tests
{
    [Collection("ApiCollection")]
    public class GetImageTest(ITestOutputHelper testOutputHelper, WebApplicationFactoryFixture factory) : Test(testOutputHelper, factory)
    {
        [Fact]
        public async Task GivenImageGroup_WhenGetImage_IsGot()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            var imageGroup = await apiClient.SaveImageGroup(imagePath).To<ImageGroup>();

            //When
            var image = await apiClient.GetImage(imageGroup.Images.First().Id).To<Image>();

            //Then
            var uploadedImageBytes = File.ReadAllBytes(imagePath);
            var downloadedImageBytes = await apiClient.HttpClient.GetByteArrayAsync(image!.Url);

            uploadedImageBytes.Should().BeEquivalentTo(downloadedImageBytes);
        }

        [Fact]
        public async Task GivenUnexistingImage_WhenGetImage_ExpectedProblemDetails()
        {
            //Given
            //When
            var response = await apiClient.GetImage(id: 600);

            //Then
            var expected = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                Title = "NotFoundException",
                Status = (int)HttpStatusCode.NotFound,
                Detail = "Image with id '600' was not found."
            };

            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
