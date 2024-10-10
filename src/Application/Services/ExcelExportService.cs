using Microsoft.EntityFrameworkCore;
using Persistence;
using SpreadCheetah;

namespace Application.Services
{
    internal class ExcelExportService(AppDbContext db)
    {
        public async Task<byte[]> Export(string tableName)
        {
            //Try do the same with dapper
            //Fix sql injection
            var query = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = {tableName}";
            var columns = await db.Set<object>().FromSqlRaw(query).ToListAsync();

            query = $"SELECT * FROM {tableName}";
            var rows = await db.Set<object>().FromSqlRaw(query).ToListAsync();

            using var stream = new MemoryStream();
            using var spreadsheet = await Spreadsheet.CreateNewAsync(stream);
            await spreadsheet.StartWorksheetAsync(tableName);

            // Cells are inserted row by row.
            var row = new List<Cell>();
            foreach(var @object in rows)
            {
                row.Add()
            }
            {
                new Cell("Answer to the ultimate question:"),
                new Cell(42)
            };

            // Rows are inserted from top to bottom.
            await spreadsheet.AddRowAsync(row);

            // Remember to call Finish before disposing.
            // This is important to properly finalize the XLSX file.
            await spreadsheet.FinishAsync();
        }
    }
}
