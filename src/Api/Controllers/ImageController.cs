//using Microsoft.AspNetCore.Mvc;
//using Application.Services;
//using Domain.Models;

//namespace Api.Controllers
//{
//    [Route("api/v{version:apiVersion}/[controller]")]
//    public class ImageController(ImageService imageService) : ControllerBase
//    {
//        [HttpGet("{id}", Name = nameof(GetImage))]
//        public Task<Image> GetImage(long id, CancellationToken cancellationToken)
//        {
//            return imageService.GetImage(id, cancellationToken);
//        }
//    }
//}