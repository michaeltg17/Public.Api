using Microsoft.EntityFrameworkCore;
using Persistence;
using SpreadCheetah;

namespace Application.Services
{
    public class ExcelExportService(AppDbContext db)
    {
        public async Task<byte[]> Export(string tableName, CancellationToken ct)
        {
            var query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = {tableName}";
            var columns = await db.Set<object>().FromSqlRaw(query, tableName).ToListAsync(ct);

            query = "SELECT * FROM {tableName}";
            var rows = await db.Set<object>().FromSqlRaw(query, tableName).ToListAsync(ct);

            using var stream = new MemoryStream();
            using var spreadsheet = await Spreadsheet.CreateNewAsync(stream, cancellationToken: ct);
            await spreadsheet.StartWorksheetAsync(tableName, token: ct);

            // Insert the column headers
            var headerRow = new List<Cell>();
            foreach (var column in columns)
            {
                headerRow.Add(new Cell());
            }
            await spreadsheet.AddRowAsync(headerRow, ct);

            // Insert the data rows
            foreach (var rowObject in rows)
            {
                var dataRow = new List<Cell>();

                // Use reflection to get property values from the row
                var properties = rowObject.GetType().GetProperties();
                foreach (var column in columns)
                {
                    // Find the property corresponding to the column
                    //var property = //properties.FirstOrDefault(p => p.Name.Equals(column, StringComparison.OrdinalIgnoreCase));
                    //var value = property != null ? property.GetValue(rowObject)?.ToString() : string.Empty;

                    dataRow.Add(new Cell());
                }

                await spreadsheet.AddRowAsync(dataRow, ct);
            }

            await spreadsheet.FinishAsync(ct);
            return stream.ToArray();
        }
    }
}
