using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Tests
{
    [Collection("ApiCollection")]
    public class GetImageTest(ITestOutputHelper testOutputHelper, WebApplicationFactoryFixture factory) : Test(testOutputHelper, factory)
    {
        [Fact]
        public async Task GivenImage_WhenSave_IsSaved()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            var imageGroup = await apiClient.SaveImageGroup(imagePath);

            //When
            var image = await apiClient.GetImage(imageGroup.Images.First().Id);

            //Then
            var uploadedImageBytes = File.ReadAllBytes(imagePath);

            using var httpClient = new HttpClient();
            var downloadedImageBytes = await apiClient.HttpClient.GetByteArrayAsync(image!.Url);
            uploadedImageBytes.Should().BeEquivalentTo(downloadedImageBytes);
        }
    }
}
