using Domain.Models;
using FluentAssertions;
using RestSharp;
using Xunit;

namespace FunctionalTests
{
    public class SaveImageTest
    {
        [Fact]
        public async Task GivenImage_WhenSave_IsSaved()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            var client = new RestClient();
            var request = new RestRequest("https://localhost:7080/SaveImageGroup");
            request.AddFile("file", imagePath);

            //When
            var response = await client.PostAsync<ImageGroup>(request);

            //Then
            var uploadedImageBytes = File.ReadAllBytes(imagePath);
            var image = response!.Images.First();

            using var httpClient = new HttpClient();
            var downloadedImageBytes = await httpClient.GetByteArrayAsync(image.Url);

            uploadedImageBytes.Should().BeEquivalentTo(downloadedImageBytes);
        }
    }
}
