using FluentAssertions;
using FunctionalTests.Others;
using Xunit;

namespace FunctionalTests
{
    public class GetImageTest : Test
    {
        [Fact]
        public async Task GivenImageGroup_WhenGetImage_IsGot()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            var imageGroup = await apiClient.SaveImageGroup(imagePath);

            //When
            var image = await apiClient.GetImage(imageGroup.Images.First().Id);

            //Then
            var uploadedImageBytes = File.ReadAllBytes(imagePath);
            var downloadedImageBytes = await apiClient.HttpClient.GetByteArrayAsync(image!.Url);

            uploadedImageBytes.Should().BeEquivalentTo(downloadedImageBytes);
        }
    }
}
