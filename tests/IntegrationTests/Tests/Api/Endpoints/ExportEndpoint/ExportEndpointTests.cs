using FluentAssertions;
using Xunit;

namespace IntegrationTests.Tests.Api.Endpoints.ExportEndpoint
{
    [Collection(nameof(ApiCollection))]
    public class ExportEndpointTests : Test
    {
        [InlineData("Images")]
        [InlineData("ImageGroups")]
        [Theory]
        public async Task ExportTableNameWorks(string tableName)
        {
            //When
            var file = (await ApiClient.MinimalApi.Export(tableName)).Content.ReadAsByteArrayAsync();

            //Then
            file.Should().NotBeNull();
        }
    }
}
