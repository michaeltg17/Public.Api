using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Domain.Models;
using Api.Filters;

namespace Api.Controllers
{
    public class ImageController(ImageService imageService) : ControllerBase
    {
        [HttpGet("api/v1/Image/{id}")]
        public Task<Image> GetImage(long id, CancellationToken cancellationToken)
        {
            return imageService.GetImage(id, cancellationToken);
        }

        [TypeFilter<SampleFilter>]
        [HttpGet("api/v1/ImageGroup/{id}")]
        public async Task<ImageGroup> GetImageGroup(long id, CancellationToken cancellationToken)
        {
            return await imageService.GetImageGroup(id, cancellationToken);
        }

        [SampleFilter]
        [HttpPost("api/v1/ImageGroup")]
        public async Task<ActionResult<ImageGroup>> SaveImageGroup(IFormFile file)
        {
            var imageGroup = await imageService.SaveImageGroup(file.FileName, () => file.OpenReadStream());
            return CreatedAtAction(nameof(GetImageGroup), new { imageGroup.Id }, imageGroup);
        }

        [HttpDelete("api/v1/ImageGroup/{id}")]
        public Task DeleteImageGroup(long id)
        {
            return imageService.DeleteImageGroup(id);
        }
    }
}