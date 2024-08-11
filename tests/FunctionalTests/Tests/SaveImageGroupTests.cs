using ApiClient.Extensions;
using Common.Testing.Models;
using FluentAssertions;
using System.Net;
using Xunit;
using FunctionalTests.Settings;

namespace FunctionalTests.Tests
{
    public class SaveImageGroupTests(ITestSettings settings) : Test(settings)
    {
        [Fact]
        public async Task GivenImage_WhenSaveImageGroup_IsSaved()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";

            //When
            var response = await apiClient.Api.SaveImageGroup(imagePath);
            var imageGroup = await response.To<ImageGroup>();

            //Then
            var imageGroup2 = await apiClient.Api.GetImageGroup(imageGroup.Id).To<ImageGroup>();
            imageGroup.Should().BeEquivalentTo(imageGroup2);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
