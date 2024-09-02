using Api.Extensions;
using Application.Services;

namespace Api.Endpoints.ImageGroup
{
    public static class DeleteImageGroupV2Endpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapDelete("ImageGroup/{id}", (
                ImageService imageService,
                long id,
                CancellationToken cancellationToken) =>
            {
                return imageService.DeleteImageGroup(id);
            })
            .WithMinimalApiName("DeleteImageGroup")
            .WithOpenApi();
        }
    }
}
