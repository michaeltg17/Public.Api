using Api.Extensions;
using Application.Services;

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
                const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return Results.File(fileContent, contentType, $"{tableName}.xlsx");
            })
            .WithMinimalApiName("Export")
            .WithOpenApi();
        }
    }
}
