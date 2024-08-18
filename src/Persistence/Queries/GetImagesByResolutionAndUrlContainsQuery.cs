using Dapper;
using Domain.Models;
using Core.Persistence;
using System.Data;

namespace Persistence.Queries
{
    public class GetImagesByResolutionAndUrlContains(ImageResolution resolution, string urlContent) : IQuery<IEnumerable<Image>>
    {
        public Task<IEnumerable<Image>> Execute(IDbConnection connection)
        {
            var sql = "SELECT * FROM Images WHERE Resolution = @resolution AND Url LIKE '@urlContent'";

            return connection.QueryAsync<Image>(sql, new { resolution, urlContent });
        }
    }
}
