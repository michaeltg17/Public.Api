using Api;
using Domain.Models;
using FluentAssertions;
using IntegrationTests.Others;
using Michael.Net.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    [Collection("ApiCollection")]
    public class GetImageTest
    {
        readonly WebApplicationFactory<Program> factory;

        public GetImageTest(ITestOutputHelper testOutputHelper, WebApplicationFactoryFixture factory)
        {
            factory.TestOutputHelper = testOutputHelper;
            this.factory = factory;
        }

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

            var downloadedImageBytes = await client.GetByteArrayAsync(image.Url);

            uploadedImageBytes.Should().BeEquivalentTo(downloadedImageBytes);
        }
    }
}
