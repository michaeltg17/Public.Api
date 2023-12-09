using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Application.Services;

namespace Api.Controllers
{
    [ApiController]
    public class ImageController : ControllerBase
    {
        readonly IImageService imageService;

        public ImageController(IImageService imageService)
        {
            this.imageService = imageService;
        }

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
        public async Task<ImageGroup> SaveImageGroup(IFormFile file)
        {
            return await imageService.SaveImageGroup(file.FileName, () => file.OpenReadStream());
        }

        [HttpDelete("ImageGroup/{id}")]
        public async Task DeleteImageGroup(long id)
        {
            await imageService.DeleteImageGroup(id);
        }
    }
}