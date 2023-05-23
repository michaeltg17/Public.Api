using Dapper;
using Domain.Models;
using Michael.Net.Persistence;
using System.Data;

namespace Persistence.Queries
{
    public class GetImagesByResolutionAndUrlContains : IQuery<IEnumerable<Image>>
    {
        readonly ImageResolution resolution;
        readonly string urlContent;

        public GetImagesByResolutionAndUrlContains(
            ImageResolution resolution,
            string urlContent)
        {
            this.resolution = resolution;
            this.urlContent = urlContent;
        }

        public Task<IEnumerable<Image>> Execute(IDbConnection connection)
        {
            var sql = "SELECT * FROM Images WHERE Resolution = @resolution AND Url LIKE '@urlContent'";

            return connection.QueryAsync<Image>(sql, new { resolution, urlContent });
        }
    }
}
