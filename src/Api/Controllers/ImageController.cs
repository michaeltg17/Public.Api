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

        [HttpGet("GetImage")]
        public async Task<Image> GetImage([FromQuery] long id)
        {
            return await imageService.GetImage(id);
        }


        [HttpGet("GetImageGroup")]
        public async Task<ImageGroup> GetImageGroup([FromQuery] long id)
        {
            return await imageService.GetImageGroup(id);
        }

        [HttpPost("SaveImageGroup")]
        public async Task<ImageGroup> SaveImageGroup(IFormFile file)
        {
            return await imageService.SaveImageGroup(file.FileName, () => file.OpenReadStream());
        }

        [HttpDelete("DeleteImageGroup")]
        public void DeleteImageGroup([FromQuery] long id)
        {
            imageService.DeleteImageGroup(id);
        }
    }
}