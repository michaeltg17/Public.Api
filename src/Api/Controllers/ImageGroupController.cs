using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Domain.Models;
using Asp.Versioning;

namespace Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1), ApiVersion(2)]
    public class ImageGroupController(ImageService imageService) : ControllerBase
    {
        [HttpGet("{id}", Name = nameof(GetImageGroup))]
        public async Task<ImageGroup> GetImageGroup(long id, CancellationToken cancellationToken)
        {
            return await imageService.GetImageGroup(id, cancellationToken);
        }

        [HttpPost(Name = nameof(SaveImageGroup))]
        public async Task<ActionResult<ImageGroup>> SaveImageGroup(IFormFile file)
        {
            var imageGroup = await imageService.SaveImageGroup(file.FileName, () => file.OpenReadStream());
            return CreatedAtAction(nameof(GetImageGroup), new { imageGroup.Id }, imageGroup);
        }

        [HttpDelete("{id}", Name = nameof(DeleteImageGroup))]
        public Task DeleteImageGroup(long id)
        {
            return imageService.DeleteImageGroup(id);
        }

        [HttpDelete(Name = nameof(DeleteImageGroupV2)), MapToApiVersion(2)]
        public Task DeleteImageGroupV2(long id)
        {
            return imageService.DeleteImageGroup(id);
        }
    }
}