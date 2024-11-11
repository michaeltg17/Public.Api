using Microsoft.EntityFrameworkCore;
using Persistence;
using SpreadCheetah;
using System.Data.Common;
using System.Data;
using Application.Extensions;
using File = Application.Models.Responses.File;
using MimeMapping;

namespace Application.Services
{
    public class ExcelExportService(AppDbContext dbContext)
    {
        public async Task<File> Export(string tableName, CancellationToken ct)
        {
            var reader = await ExecuteQuery(tableName, ct);

            //Create Excel
            using var stream = new MemoryStream();
            using var spreadsheet = await Spreadsheet.CreateNewAsync(stream, cancellationToken: ct);
            await spreadsheet.StartWorksheetAsync(tableName, token: ct);

            //Add columns
            var columnsRow = reader.GetColumnSchema().Select(c => new Cell(c.ColumnName)).ToList();
            await spreadsheet.AddRowAsync(columnsRow, ct);

            //Add rows
            while (await reader.ReadAsync(ct))
            {
                var row = new List<Cell>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    row.Add(value.ToCell());
                }
                await spreadsheet.AddRowAsync(row, ct);
            }

            await spreadsheet.FinishAsync(ct);

            return new File
            {
                Content = stream.ToArray(),
                ContentType = KnownMimeTypes.Xlsx,
                Name = $"{tableName}.xlsx"
            };
        }

        async Task<DbDataReader> ExecuteQuery(string tableName, CancellationToken ct)
        {
            var connection = dbContext.Database.GetDbConnection();
            DbProviderFactory factory = DbProviderFactories.GetFactory(connection)!;
            DbCommandBuilder commandBuilder = factory.CreateCommandBuilder()!;

            var sanitizedTableName = commandBuilder.QuoteIdentifier(tableName);
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM " + sanitizedTableName;

            await connection.OpenAsync(ct);
            return await command.ExecuteReaderAsync(ct);
        }
    }
}
