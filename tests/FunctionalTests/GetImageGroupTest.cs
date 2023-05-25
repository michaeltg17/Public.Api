using Domain.Models;
using FluentAssertions;
using RestSharp;
using Xunit;

namespace FunctionalTests
{
    public class GetImageGroupTest
    {
        [Fact]
        public async Task GivenImage_WhenSaveAndGetImageGroup_IsGot()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";

            var client = new RestClient();
            var request = new RestRequest("https://localhost:7080/SaveImageGroup");
            request.AddFile("file", imagePath);
            var group = await client.PostAsync<ImageGroup>(request);

            //When
            request = new RestRequest("https://localhost:7080/GetImageGroup");
            request.AddParameter("id", group!.Id);
            group = await client.GetAsync<ImageGroup>(request);

            //Then2
            var image = group!.Images.First();
            var uploadedImageBytes = File.ReadAllBytes(imagePath);

            using var httpClient = new HttpClient();
            var downloadedImageBytes = await httpClient.GetByteArrayAsync(image.Url);

            uploadedImageBytes.Should().BeEquivalentTo(downloadedImageBytes);
            group.Images.Should().NotBeNullOrEmpty();
        }
    }
}
