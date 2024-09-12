using Api.Extensions;
using Application.Services;

namespace Api.Endpoints.Image
{
    public static class GetImageEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapGet("Image/{id}", (
                ImageService imageService,
                long id,
                CancellationToken cancellationToken) =>
            {
                return imageService.GetImage(id, cancellationToken);
            })
            .WithMinimalApiName("GetImage")
            .WithOpenApi();
        }
    }
}
