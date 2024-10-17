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
            //add data
            
            //When
            var response = await ApiClient.MinimalApi.Export(tableName);

            //Then
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK);

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
