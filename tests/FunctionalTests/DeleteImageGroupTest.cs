using Domain.Models;
using FluentAssertions;
using FunctionalTests.Others;
using RestSharp;
using Xunit;

namespace FunctionalTests
{
    public class DeleteImageGroupTest : ApiTest
    {
        [Fact]
        public async Task GivenImageGroup_WhenDelete_IsDeleted()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            var client = new RestClient();
            var request = new RestRequest(Routes.SaveImageGroup);
            request.AddFile("file", imagePath);
            var response = await client.PostAsync<ImageGroup>(request);

            var request = new RestRequest(Routes.SaveImageGroup);

            //When
            request = new RestRequest(Routes.DeleteImageGroup);
            request.AddParameter("id", response!.Id);
            await client.PostAsync(request);

            //Then

            var uploadedImageBytes = File.ReadAllBytes(imagePath);
            var image = response!.Images.First();

            using var httpClient = new HttpClient();
            var downloadedImageBytes = await httpClient.GetByteArrayAsync(image.Url);

            uploadedImageBytes.Should().BeEquivalentTo(downloadedImageBytes);
        }
    }
}
