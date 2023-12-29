using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Application.Services;

namespace Api.Controllers
{
    public class ImageController(IImageService imageService) : ControllerBase
    {
        [HttpGet("Image/{id}")]
        public Task<Image> GetImage(long id, CancellationToken cancellationToken)
        {
            return imageService.GetImage(id, cancellationToken);
        }

        [HttpGet("ImageGroup/{id}")]
        public Task<ImageGroup> GetImageGroup(long id, CancellationToken cancellationToken)
        {
            return imageService.GetImageGroup(id, cancellationToken);
        }

        [HttpPost("ImageGroup")]
        public async Task<ActionResult<ImageGroup>> SaveImageGroup(IFormFile file)
        {
            var imageGroup = await imageService.SaveImageGroup(file.FileName, () => file.OpenReadStream());
            return CreatedAtAction(nameof(GetImageGroup), new { imageGroup.Id }, imageGroup);
        }

        [HttpDelete("ImageGroup/{id}")]
        public Task DeleteImageGroup(long id)
        {
            return imageService.DeleteImageGroup(id);
        }
    }
}