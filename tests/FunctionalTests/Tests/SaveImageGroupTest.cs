using Client;
using Domain.Models;
using FluentAssertions;
using System.Net;
using Xunit;

namespace FunctionalTests.Tests
{
    public class SaveImageGroupTest(ISettings settings) : Test(settings)
    {
        [Fact]
        public async Task GivenImage_WhenSaveImageGroup_IsSaved()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";

            //When
            var response = await apiClient.SaveImageGroup(imagePath);
            var imageGroup = await response.To<ImageGroup>();

            //Then
            var imageGroup2 = await apiClient.GetImageGroup(imageGroup.Id).To<ImageGroup>();
            imageGroup.Should().BeEquivalentTo(imageGroup2);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task GivenBadRquest_WhenSaveImageGroup_ExpectedProblemDetails()
        {
            //Given
            var x = await apiClient.HttpClient.PostAsync("ImageGroup", null);

            //When
            //var response = await apiClient.SaveImageGroup(imagePath);
            //var imageGroup = response.To<ImageGroup>();

            ////Then
            //var imageGroup2 = await apiClient.GetImageGroup(imageGroup.Id);
            //imageGroup.Should().BeEquivalentTo(imageGroup2);
            //response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
