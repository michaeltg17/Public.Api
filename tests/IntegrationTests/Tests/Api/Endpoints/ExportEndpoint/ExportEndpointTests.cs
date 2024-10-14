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
            //When
            var response = await ApiClient.MinimalApi.Export(tableName);

            //Then
            var fileName = $"{tableName}.xlsx";
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK);
            response.Content.Headers.ContentDisposition!.FileName.Should().Be(fileName);
            var file = await response.Content.ReadAsByteArrayAsync();
            var expectedFile = File.ReadAllBytes(GetTestFilePath(fileName));
            file.Should().BeEquivalentTo(expectedFile);
        }
    }
}
