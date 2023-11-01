using Domain.Models;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System;
using Persistence.Queries;
using Michael.Net.Persistence;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;

namespace Application.Services
{
    public class ImageService : IImageService
    {
        readonly AppDbContext dbContext;
        readonly IObjectStorage objectStorage;

        public ImageService(IObjectStorage objectStorage, AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.objectStorage = objectStorage;
        }

        public async Task<Image> GetImage(int id)
        {
            return await dbContext.Images.SingleAsync(x => x.Id == id);
        }

        public async Task<ImageGroup> GetImageGroup(int id)
        {
            return await dbContext.Get(new GetImageGroupWithImagesQuery(id));
        }

        public async Task<ImageGroup> SaveImageGroup(string fullFileName, Func<Stream> openReadStream)
        {
            var images = await SaveImageWithMultipleResolutions(fullFileName, openReadStream);

            var imageGroup = new ImageGroup()
            {
                Name = Path.GetFileNameWithoutExtension(fullFileName),
                Images = images
            };
            await dbContext.AddAsync(imageGroup);
            await dbContext.SaveChangesAsync();

            return imageGroup;
        }

        async Task<Image> SaveImageFile(string fullFileName, Stream stream, ImageResolution resolution)
        {
            var image = new Image
            {
                Resolution = resolution.Id,
                Url = await objectStorage.Upload(fullFileName, stream)
            };

            await stream.DisposeAsync();

            return image;
        }

        async Task<IEnumerable<Image>> SaveImageWithMultipleResolutions(string fullFileName, Func<Stream> openReadStream)
        {
            var tasks = new List<Task<Image>>();
            using (var stream = openReadStream())
            {
                foreach (var resolution in dbContext.ImageResolutions)
                {
                    var memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    tasks.Add(SaveImageFile(BuildName(fullFileName), memoryStream, resolution));
                    stream.Position = 0;
                }
            }

            return await Task.WhenAll(tasks);
        }

        string BuildName(string fullFileName)
        {
            var extension = Path.GetExtension(fullFileName);
            return $"{Guid.NewGuid()}{extension}";
        }

        public void DeleteImageGroup(int id)
        {
            dbContext.Remove<ImageGroup>(id);
        }
    }
}
