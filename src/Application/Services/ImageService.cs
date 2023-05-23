using Domain.Models;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System;
using Persistence.Queries;
using Persistence;
using Michael.Net.Persistence;

namespace Application.Services
{
    public class ImageService : IImageService
    {
        readonly IUnitOfWork unitOfWork;
        readonly IObjectStorage objectStorage;
        readonly IRepository repository;

        public ImageService(
            IUnitOfWork unitOfWork,
            IObjectStorage objectStorage,
            IRepository repository)
        {
            this.unitOfWork = unitOfWork;
            this.objectStorage = objectStorage;
            this.repository = repository;
        }

        public async Task<Image> GetImage(int id)
        {
            return await repository.GetOrThrow<Image>(id);
        }

        public async Task<ImageGroup> GetImageGroup(int id)
        {
            return await repository.Get(new GetImageGroupWithImagesQuery(id));
        }

        public async Task<ImageGroup> SaveImageGroup(string fullFileName, Func<Stream> openReadStream)
        {
            using var transaction = await unitOfWork.BeginTransaction();

            var images = await SaveImageWithMultipleResolutions(fullFileName, openReadStream);

            var imageGroup = new ImageGroup()
            {
                Name = Path.GetFileNameWithoutExtension(fullFileName),
                Images = new List<Image>()
            };
            await repository.Insert(imageGroup);

            foreach (var image in images)
            {
                image.ImageGroupId = imageGroup.Id;
                await repository.Insert(image);
                imageGroup.Images.Add(image);
            }

            transaction.Complete();

            return imageGroup;
        }

        async Task<Image> SaveImageFile(string fullFileName, Stream stream, ImageResolution resolution)
        {
            var image = new Image
            {
                ImageResolutionId = resolution.Id,
                Url = await objectStorage.Upload(fullFileName, stream)
            };

            await stream.DisposeAsync();

            return image;
        }

        async Task<IEnumerable<Image>> SaveImageWithMultipleResolutions(string fullFileName, Func<Stream> openReadStream)
        {
            var resolutions = await repository.Get<ImageResolution>();

            var tasks = new List<Task<Image>>();
            using (var stream = openReadStream())
            {
                foreach (var resolution in resolutions)
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

        public async Task DeleteImageGroup(int id)
        {
            await repository.Delete<ImageGroup>(id);
        }
    }
}
