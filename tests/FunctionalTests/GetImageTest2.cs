﻿using Domain.Models;
using FluentAssertions;
using RestSharp;
using Xunit;

namespace FunctionalTests
{
    public class GetImageTest2
    {
        [Fact]
        public async Task GivenImage_WhenSaveAndGet_IsGot2()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";

            var client = new RestClient();
            var request = new RestRequest("https://localhost:7080/SaveImageGroup");
            request.AddFile("file", imagePath);
            var group = await client.PostAsync<ImageGroup>(request);

            //When
            request = new RestRequest("https://localhost:7080/GetImage");
            request.AddParameter("id", group!.Images.First().Id);
            var image = await client.GetAsync<Image>(request);

            //Then
            var uploadedImageBytes = File.ReadAllBytes(imagePath);

            using var httpClient = new HttpClient();
            var downloadedImageBytes = await httpClient.GetByteArrayAsync(image!.Url);

            uploadedImageBytes.Should().BeEquivalentTo(downloadedImageBytes);
        }
    }
}