using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Application.Services;

namespace ImageSharingPlatform.Controllers
{
    [ApiController]
    public class ImageController : ControllerBase
    {
        readonly IImageService imageService;

        public ImageController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [HttpGet("GetImage")]
        public async Task<Image> GetImage(int id)
        {
            return await imageService.GetImage(id);
        }


        [HttpGet("GetImageGroup")]
        public async Task<ImageGroup> GetImageGroup(int id)
        {
            return await imageService.GetImageGroup(id);
        }

        [HttpPost("SaveImageGroup")]
        public async Task<ImageGroup> SaveImageGroup(IFormFile file)
        {
            return await imageService.SaveImageGroup(file.FileName, () => file.OpenReadStream());
        }

        [HttpPost("DeleteImageGroup")]
        public void DeleteImageGroup(int id)
        {
            imageService.DeleteImageGroup(id);
        }
    }
}