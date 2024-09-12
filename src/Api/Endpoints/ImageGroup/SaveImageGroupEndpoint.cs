using Api.Extensions;
using Application.Services;

namespace Api.Endpoints.ImageGroup
{
    public static class SaveImageGroupEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapPost("ImageGroup", async (
                ImageService imageService,
                IFormFile file,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var imageGroup = await imageService.SaveImageGroup(file.FileName, () => file.OpenReadStream());

                var locationUri = $"/ImageGroup/{imageGroup.Id}";
                context.Response.Headers.Location = locationUri;
                return Results.Created(locationUri, imageGroup);
            })
            .WithMinimalApiName("SaveImageGroup")
            .WithOpenApi()
            .DisableAntiforgery();
        }
    }
}
