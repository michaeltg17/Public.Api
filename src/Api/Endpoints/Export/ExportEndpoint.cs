using Api.Extensions;
using Application.Services;
using MimeMapping;

namespace Api.Endpoints.Export
{
    public static class ExportEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapGet("Export/{tableName}", async (
                ExcelExportService excelExportService,
                string tableName,
                CancellationToken cancellationToken) =>
            {
                var fileContent = await excelExportService.Export(tableName, cancellationToken);
                const string contentType = KnownMimeTypes.Xlsx;
                return Results.File(fileContent, contentType, $"{tableName}.xlsx");
            })
            .WithMinimalApiName("Export")
            .WithOpenApi();
        }
    }
}
