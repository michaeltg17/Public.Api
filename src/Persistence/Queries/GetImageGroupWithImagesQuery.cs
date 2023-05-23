using Dapper;
using Domain.Models;
using Michael.Net.Persistence;
using System.Data;

namespace Persistence.Queries
{
    public class GetImageGroupWithImagesQuery : IQuery<ImageGroup>
    {
        readonly long imageGroupId;

        public GetImageGroupWithImagesQuery(long imageGroupId)
        {
            this.imageGroupId = imageGroupId;
        }

        public async Task<ImageGroup> Execute(IDbConnection connection)
        {
            var sql =
                @$"
                    SELECT * FROM Images WHERE ImageGroupId = @{nameof(imageGroupId)};
                    SELECT * FROM ImageGroups WHERE Id = @{nameof(imageGroupId)};
                ";

            var grids = await connection.QueryMultipleAsync(
                sql,
                new { imageGroupId });

            var images = grids.Read<Image>();
            var imageGroup = grids.ReadSingle<ImageGroup>();
            imageGroup.Images = images.AsList();

            return imageGroup;
        }
    }
}
