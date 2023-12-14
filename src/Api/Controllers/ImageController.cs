using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Application.Services;

namespace Api.Controllers
{
    [ApiController]
    public class ImageController(IImageService imageService) : ControllerBase
    {
        [HttpGet("Image/{id}")]
        public async Task<Image> GetImage(long id)
        {
            return await imageService.GetImage(id);
        }

        [HttpGet("ImageGroup/{id}")]
        public async Task<ImageGroup> GetImageGroup(long id)
        {
            return await imageService.GetImageGroup(id);
        }

        [HttpPost("ImageGroup")]
        public async Task<ActionResult<ImageGroup>> SaveImageGroup(IFormFile file)
        {
            var imageGroup = await imageService.SaveImageGroup(file.FileName, () => file.OpenReadStream());
            return CreatedAtAction(nameof(GetImageGroup), new { imageGroup.Id }, imageGroup);
        }

        [HttpDelete("ImageGroup/{id}")]
        public async Task DeleteImageGroup(long id)
        {
            await imageService.DeleteImageGroup(id);
        }
    }
}