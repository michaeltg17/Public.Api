using Core.Testing.Extensions;
using Core.Testing.Helpers;
using FluentAssertions;
using System.Net;
using Xunit;

namespace IntegrationTests.Tests.Api.Endpoints.ExportEndpoint
{
    [Collection(nameof(ApiCollection))]
    public class ExportEndpointTests : Test
    {
        [InlineData("Images")]
        [InlineData("ImageGroups")]
        [Theory]
        public async Task ExportWorks(string tableName)
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            await ApiClient.MinimalApi.SaveImageGroup(imagePath).EnsureSuccessStatusCode();

            //When
            var response = await ApiClient.MinimalApi.Export(tableName);

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var fileName = $"{tableName}.xlsx";
            response.Content.Headers.ContentDisposition!.FileName.Should().Be(fileName);

            var file = await response.Content.ReadAsByteArrayAsync();
            await TestFileHelper.OpenFile(file, fileName);
            var expectedFilePath = TestFileHelper.GetTestFilePath(GetType(), fileName);
            var expectedFile = await File.ReadAllBytesAsync(expectedFilePath);
            file.Should().BeEquivalentTo(expectedFile);
        }
    }
}
