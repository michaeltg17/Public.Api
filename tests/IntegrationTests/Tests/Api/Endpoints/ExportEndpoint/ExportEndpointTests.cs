using ApiClient.Extensions;
using ClosedXML.Excel;
using Core.Extensions;
using Domain.Models;
using FluentAssertions;
using MoreLinq;
using System.Net;
using Xunit;
using IntegrationTests.Extensions;

namespace IntegrationTests.Tests.Api.Endpoints.ExportEndpoint
{
    [Collection(nameof(ApiCollection))]
    public class ExportEndpointTests : Test
    {
        [Fact]
        public async Task ExportImageGroupsWorks()
        {
            //Given
            IEnumerable<string> imageGroupColummsOrder = [
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
            const string tableName = "ImageGroups";
            const string imagePath = @"Images\didi.jpeg";
            var imageGroup = await ApiClient.MinimalApi.SaveImageGroup(imagePath).To<ImageGroup>();

            //When
            var response = await ApiClient.MinimalApi.Export(tableName);

            //Then
            await ValidateResponse(response, tableName, imageGroupColummsOrder, [imageGroup]);
        }

        [Fact]
        public async Task ExportImagesWorks()
        {
            //Given
            string[] imageColummsOrder = [
                nameof(Image.Id),
                nameof(Image.Guid),
                nameof(Image.Url),
                nameof(Image.Resolution),
                nameof(Image.Group),
                nameof(Image.CreatedBy),
                nameof(Image.CreatedOn),
                nameof(Image.ModifiedBy),
                nameof(Image.ModifiedOn),
                nameof(Image.IsTest),
            ];
            const string tableName = "Images";
            const string imagePath = @"Images\didi.jpeg";
            var imageGroup = await ApiClient.MinimalApi.SaveImageGroup(imagePath).To<ImageGroup>();

            //When
            var response = await ApiClient.MinimalApi.Export(tableName);

            //Then
            await ValidateResponse(response, tableName, imageColummsOrder, imageGroup.ImagesNavigation);
        }

        static async Task ValidateResponse<T>(
            HttpResponseMessage response,
            string tableName,
            IEnumerable<string> columnNames,
            IEnumerable<T> entities) where T : notnull
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var fileName = $"{tableName}.xlsx";
            response.Content.Headers.ContentDisposition!.FileName.Should().Be(fileName);

            var file = await response.Content.ReadAsStreamAsync();
            ValidateExcel(file, columnNames, entities);
        }

        static void ValidateExcel<T>(Stream file, IEnumerable<string> columns, IEnumerable<T> entities) where T : notnull
        {
            using var workbook = new XLWorkbook(file);
            var worksheet = workbook.Worksheets.Single();
            var usedCells = new Dictionary<int, int>();

            //Validate columns
            columns.For(1, (i, columnName) => worksheet.Cell(1, i).GetValue<string>().Should().Be(columnName));

            //Validate rows
            entities.For(2, (rowIndex, entity) =>
            {
                //Join ordered columns with values
                var columnsWithValue = columns
                    .Join(
                        entity.GetType().GetProperties(),
                        c => c,
                        p => p.Name,
                        (c, p) => new { Name = c, Type = p.PropertyType, Value = p.GetValue(entity) });
                columnsWithValue.Should().NotBeEmpty();

                //Validate cells
                columnsWithValue.For(1, (columnIndex, c) => worksheet
                    .Cell(rowIndex, columnIndex)
                    .GetValue(c.Type)
                    .Should()
                    .Be(c.Value));
            });

            //Validate other cells are empty
            worksheet.LastColumnUsed()!.ColumnNumber().Should().Be(columns.Count());
            worksheet.LastRowUsed()!.RowNumber().Should().Be(entities.Count() + 1);
        }
    }
}
