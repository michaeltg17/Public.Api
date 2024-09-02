using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Domain.Models;
using Api.Abstractions;

namespace Api.Controllers
{
    [Route("ControllerApi/api/v{version:apiVersion}/[controller]")]
    public class ImageController(ImageService imageService) : ControllerBase
    {
        const string NamePrefix = "ControllerApi" + ".";

        [HttpGet("{id}", Name = NamePrefix + nameof(GetImage))]
        public Task<Image> GetImage(long id, CancellationToken cancellationToken)
        {
            return imageService.GetImage(id, cancellationToken);
        }
    }
}