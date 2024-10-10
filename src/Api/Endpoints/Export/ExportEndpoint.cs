using Api.Extensions;
using Application.Services;

namespace Api.Endpoints.Image
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
                return Results.File(await excelExportService.Export(tableName, cancellationToken));
            })
            .WithMinimalApiName("Export")
            .WithOpenApi();
        }
    }
}
