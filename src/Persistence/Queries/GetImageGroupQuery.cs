﻿using Dapper;
using Domain.Models;
using Michael.Net.Persistence;
using System.Data;

namespace Persistence.Queries
{
    public class GetImageGroupQuery : IQuery<ImageGroup>
    {
        readonly long id;

        public GetImageGroupQuery(long id)
        {
            this.id = id;
        }

        public async Task<ImageGroup> Execute(IDbConnection connection)
        {
            var sql =
                @$"
                    SELECT * FROM Images WHERE [Group] = @{nameof(id)};
                    SELECT * FROM ImageGroups WHERE Id = @{nameof(id)};
                ";

            var grids = await connection.QueryMultipleAsync(
                sql,
                new { id });

            var images = grids.Read<Image>();
            var imageGroup = grids.ReadSingle<ImageGroup>();
            imageGroup.ImagesNavigation = images.AsList();

            return imageGroup;
        }
    }
}
