using FluentAssertions;
using Xunit;

namespace FunctionalTests.Tests
{
    public class GetImageTest(ISettings settings) : Test(settings)
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
