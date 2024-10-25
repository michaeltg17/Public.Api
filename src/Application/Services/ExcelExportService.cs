using Microsoft.EntityFrameworkCore;
using Persistence;
using SpreadCheetah;
using static Application.Extensions.SpreadCheetahExtensions;
using System.Data.Common;
using System.Data;
using Core.Extensions;

namespace Application.Services
{
    public class ExcelExportService(AppDbContext dbContext)
    {
        public async Task<byte[]> Export(string tableName, CancellationToken ct)
        {
            var (columns, rows) = await GetData(tableName, ct);

            using var stream = new MemoryStream();
            using var spreadsheet = await Spreadsheet.CreateNewAsync(stream, cancellationToken: ct);
            await spreadsheet.StartWorksheetAsync(tableName, token: ct);

            //Add columns
            var columnsRow = columns.Select(c => new Cell(c)).ToList();
            await spreadsheet.AddRowAsync(columnsRow, ct);

            //Add rows
            rows.ForEach(async row =>
            {
                var dataRow = row.Select(value => CreateCell(value)).ToList();
                await spreadsheet.AddRowAsync(dataRow, ct);
            });

            await spreadsheet.FinishAsync(ct);
            return stream.ToArray();
        }

        async Task<(IEnumerable<string> columns, IEnumerable<IEnumerable<object?>> rows)> GetData(
            string tableName, CancellationToken ct)
        {
            var connection = dbContext.Database.GetDbConnection();
            DbProviderFactory factory = DbProviderFactories.GetFactory(connection)!;
            DbCommandBuilder commandBuilder = factory.CreateCommandBuilder()!;

            var sanitizedTableName = commandBuilder.QuoteIdentifier(tableName);
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM " + sanitizedTableName;

            await connection.OpenAsync(ct);
            using var reader = await command.ExecuteReaderAsync(ct);
            var columns = reader.GetColumnSchema().Select(c => c.ColumnName).ToList();

            var rows = new List<List<object?>>();
            while (await reader.ReadAsync(ct))
            {
                var row = new List<object?>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    row.Add(value);
                }
                rows.Add(row);
            }

            return (columns, rows);
        }
    }
}
