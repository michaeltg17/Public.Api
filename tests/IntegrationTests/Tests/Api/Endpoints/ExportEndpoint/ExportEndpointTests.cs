using ClosedXML.Excel;
using Core.Extensions;
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
            ValidateExcel
            await TestFileHelper.OpenFile(file, fileName);
            var expectedFilePath = TestFileHelper.GetTestFilePath(GetType(), fileName);
            var expectedFile = await File.ReadAllBytesAsync(expectedFilePath);
            file.Should().BeEquivalentTo(expectedFile);
        }

        static void ValidateExcel<T>(Stream file, IEnumerable<T> entities) where T : notnull
        {
            using var workbook = new XLWorkbook(file);
            var worksheet = workbook.Worksheets.Single();

            typeof(T)
                .GetProperties()
                .Select(p => p.Name)
                .For((columnIndex, columnName) => worksheet.Cell(1, columnIndex).Value.Should().Be(columnName));

            entities.ForEach(entity => 
            {
                entity
                    .GetType()
                    .GetProperties()
                    .Select(p => p.GetValue(entity))
                    .For((rowIndex, value) => worksheet.Cell(1, rowIndex).Value.Should().Be(value));
            });
        }
    }
}
