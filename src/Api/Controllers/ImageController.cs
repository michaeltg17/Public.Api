using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Domain.Models;
using Api.Abstractions;

namespace Api.Controllers
{
    public class ImageController(ImageService imageService) : ControllerBaseCustom
    {
        [HttpGet("{id}", Name = NamePrefix + nameof(GetImage))]
        public Task<Image> GetImage(long id, CancellationToken cancellationToken)
        {
            return imageService.GetImage(id, cancellationToken);
        }
    }
}