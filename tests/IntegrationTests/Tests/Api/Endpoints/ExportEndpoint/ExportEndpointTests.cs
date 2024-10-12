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
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK);
            var file = await response.Content.ReadAsByteArrayAsync();
            file.Should().NotBeNull();
        }
    }
}
