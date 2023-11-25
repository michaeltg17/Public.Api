using Api;
using Domain.Models;
using FluentAssertions;
using IntegrationTests.Other;
using Michael.Net.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    [Collection("ApiCollection")]
    public class SaveImageTest
    {
        readonly WebApplicationFactory<Program> factory;

        public SaveImageTest(ITestOutputHelper testOutputHelper, WebApplicationFactoryFixture factory)
        {
            factory.TestOutputHelper = testOutputHelper;
            this.factory = factory;
        }

        //[Fact(Skip = "Needs mock in IObjectStorage")]
        [Fact]
        public async Task GivenImage_WhenSave_IsSaved()
        {
            // Given
            var client = factory.CreateClient();
            const string imagePath = @"Images\didi.jpeg";
            var multipartContent = new MultipartFormDataContent();
            var byteArrayContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
            multipartContent.Add(byteArrayContent, "file", Path.GetFileName(imagePath));

            // When
            var response = await (await client.PostAsync("SaveImageGroup", multipartContent)).FromJson<ImageGroup>();

            // Then
            var uploadedImageBytes = File.ReadAllBytes(imagePath);
            var image = response!.Images.First();

            using var httpClient = new HttpClient();
            var downloadedImageBytes = await httpClient.GetByteArrayAsync(image.Url);

            uploadedImageBytes.Should().BeEquivalentTo(downloadedImageBytes);
        }

        [Fact]
        public async Task GivenImage_WhenSave_IsSaved2()
        {
            // Given
            var client = factory.CreateClient();

            // When
            var image = await client.GetFromJsonAsync<Image>("GetImage?id=1");

            // Then
            image.Should().NotBeNull();
        }
    }
}
