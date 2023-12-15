using Domain.Models;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using Persistence.Queries;
using Michael.Net.Persistence;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;
using System;
using Application.Exceptions;

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

        public async Task<Image> GetImage(long id)
        {
            return await dbContext.Images.SingleOrDefaultAsync(x => x.Id == id)
                ?? throw new NotFoundException<Image>(id);
        }

        public async Task<ImageGroup> GetImageGroup(long id)
        {
            return await dbContext.ImageGroups
                .Include(g => g.Images)
                .ThenInclude(i => i.ResolutionNavigation)
                .SingleOrDefaultAsync(g => g.Id == id)
                ?? throw new NotFoundException<ImageGroup>(id);
        }

        async Task<ImageGroup> GetImageGroupWithDapper(long id)
        {
            return await dbContext.Get(new GetImageGroupQuery(id));
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

        static string BuildName(string fullFileName)
        {
            var extension = Path.GetExtension(fullFileName);
            return $"{Guid.NewGuid()}{extension}";
        }

        public async Task DeleteImageGroup(long id)
        {
            dbContext.Remove<ImageGroup>(id);
            if (await dbContext.SaveChangesAsync() == 0) throw new NotFoundException<ImageGroup>(id);
        }
    }
}
