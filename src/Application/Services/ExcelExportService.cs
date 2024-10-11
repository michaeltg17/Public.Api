using Microsoft.EntityFrameworkCore;
using Persistence;
using SpreadCheetah;
using Dapper;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Application.Services
{
    public class ExcelExportService(AppDbContext db)
    {
        
        public async Task<byte[]> Export(string tableName, CancellationToken ct)
        {
            //FormattableString query = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = {tableName}";
            //var columns = await db.Database.SqlQuery<string>(query).ToListAsync(ct);

            //query = $"SELECT * FROM {tableName}";
            //var rows = await db.Database.SqlQuery<object>(query).ToListAsync(ct);
            
            //var query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName";
            //var columns = (await db.Database.GetDbConnection().QueryAsync<string>(query, new { TableName = tableName })).ToList();

            var query = $"SELECT * FROM {tableName}";
            var rows = await db.Database.GetDbConnection().QueryAsync(query);

            using var stream = new MemoryStream();
            using var spreadsheet = await Spreadsheet.CreateNewAsync(stream, cancellationToken: ct);
            await spreadsheet.StartWorksheetAsync(tableName, token: ct);

            //Columns
            var firstRow = rows.First() as IDictionary<string, object>;
            var columns = firstRow!.Keys;
            var headerRow = new List<Cell>();
            foreach (var column in columns)
            {
                headerRow.Add(new Cell(column));
            }
            await spreadsheet.AddRowAsync(headerRow, ct);

            //Rows
            foreach (IDictionary<string, object> row in rows)
            {
                var dataRow = new List<Cell>();

                foreach (var column in columns)
                {
                    row.TryGetValue(column, out object? value);
                    dataRow.Add(new Cell(value));
                }

                await spreadsheet.AddRowAsync(dataRow, ct);
            }

            await spreadsheet.FinishAsync(ct);
            return stream.ToArray();
        }
    }
}
