using ApiClient.Extensions;
using ClosedXML.Excel;
using Core.Extensions;
using Domain.Models;
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
            var imageGroup = await ApiClient.MinimalApi.SaveImageGroup(imagePath).To<ImageGroup>();

            //When
            var response = await ApiClient.MinimalApi.Export(tableName);

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var fileName = $"{tableName}.xlsx";
            response.Content.Headers.ContentDisposition!.FileName.Should().Be(fileName);

            var file = await response.Content.ReadAsStreamAsync();
            ValidateExcel(file, ImageGroupColumms, imageGroup);
        }

        static void ValidateExcel<T>(Stream file, string[] columnNames, params T[] entities) where T : notnull
        {
            using var workbook = new XLWorkbook(file);
            var worksheet = workbook.Worksheets.Single();

            columnNames.For((i, columnName) => worksheet.Cell(1, i + 1).Value.Should().Be(columnName));

            entities.For((rowIndex, entity) => 
            {
                entity
                    .GetType()
                    .GetProperties()
                    .Join(columnNames, p => p.Name, c => c, (p, c) => new { Name = c, Value = p.GetValue(c).ToString() })
                    .For((columnIndex, column) => worksheet.Cell(rowIndex, columnIndex + 1).Value.Should().Be(column.Value));
            });
        }

        readonly string[] ImageGroupColumms = [
            nameof(ImageGroup.Id),
            nameof(ImageGroup.Guid),
            nameof(ImageGroup.Name),
            nameof(ImageGroup.Type),
            nameof(ImageGroup.CreatedBy),
            nameof(ImageGroup.CreatedOn),
            nameof(ImageGroup.ModifiedBy),
            nameof(ImageGroup.ModifiedOn),
            nameof(ImageGroup.IsTest),
        ];
    }
}
