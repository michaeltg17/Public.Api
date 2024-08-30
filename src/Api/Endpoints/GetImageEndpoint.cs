using Application.Services;

namespace Api.Endpoints
{
    public static class GetImageEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapGet("Image/{id}", async (
                ImageService imageService,
                long id,
                CancellationToken cancellationToken) =>
            {
                return await imageService.GetImage(id, cancellationToken);
            })
            .WithName("GetImage")
            .WithOpenApi();
        }
    }
}
