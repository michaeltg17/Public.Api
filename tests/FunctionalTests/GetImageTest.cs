//using Domain.Models;
//using FluentAssertions;
//using FunctionalTests.Others;
//using RestSharp;
//using Xunit;

//namespace FunctionalTests
//{
//    public class GetImageTest : ApiTest
//    {
//        [Fact]
//        public async Task GivenImage_WhenSaveAndGet_IsGot()
//        {
//            //Given
//            const string imagePath = @"Images\didi.jpeg";

//            var client = new RestClient();
//            var request = new RestRequest(Routes.SaveImageGroup);
//            request.AddFile("file", imagePath);
//            var group = await client.PostAsync<ImageGroup>(request);

//            //When
//            request = new RestRequest(Routes.GetImage);
//            request.AddParameter("id", group!.Images.First().Id);
//            var image = await client.GetAsync<Image>(request);

//            //Then
//            var uploadedImageBytes = File.ReadAllBytes(imagePath);

//            using var httpClient = new HttpClient();
//            var downloadedImageBytes = await httpClient.GetByteArrayAsync(image!.Url);

//            uploadedImageBytes.Should().BeEquivalentTo(downloadedImageBytes);
//        }
//    }
//}
