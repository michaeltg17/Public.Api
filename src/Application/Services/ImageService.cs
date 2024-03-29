﻿using Domain.Models;
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
using System.Threading;
using System.Transactions;

namespace Application.Services
{
    public class ImageService(IObjectStorage objectStorage, AppDbContext db) : IImageService
    {
        public async Task<Image> GetImage(long id, CancellationToken cancellationToken)
        {
            return await db.Images.SingleOrDefaultAsync(x => x.Id == id, cancellationToken)
                ?? throw new NotFoundException<Image>(id);
        }

        public async Task<ImageGroup> GetImageGroup(long id, CancellationToken cancellationToken)
        {
            return await db.ImageGroups
                .Include(g => g.ImagesNavigation)
                .ThenInclude(i => i.ResolutionNavigation)
                .SingleOrDefaultAsync(g => g.Id == id, cancellationToken)
                ?? throw new NotFoundException<ImageGroup>(id);
        }

        async Task<ImageGroup> GetImageGroupWithDapper(long id)
        {
            return await db.Get(new GetImageGroupQuery(id));
        }

        public async Task<ImageGroup> SaveImageGroup(string fullFileName, Func<Stream> openReadStream)
        {
            var extension = Path.GetExtension(fullFileName)[1..];
            var hasValidExtension = await db.ImageTypes.AnyAsync(t => t.FileExtensionNavigation.Any(e => e.FileExtension == extension));
            if (!hasValidExtension) throw new ApiException($"Extension '{extension}' is not a valid image extension.");

            using var transaction = new TransactionScope();
            var images = await SaveImageWithMultipleResolutions(fullFileName, openReadStream);

            var imageGroup = new ImageGroup()
            {
                Name = Path.GetFileNameWithoutExtension(fullFileName),
                ImagesNavigation = images
            };
            await db.AddAsync(imageGroup);
            await db.SaveChangesAsync();

            return imageGroup;
        }

        async Task<Image> SaveImageFile(string fullFileName, Stream stream, ImageResolution resolution)
        {
            var guid = new Guid();
            var extension = Path.GetExtension(fullFileName);
            var fileName = $"{guid}{extension}";
            var image = new Image
            {
                Guid = guid,
                Resolution = resolution.Id,
                Url = await objectStorage.Upload(fileName, stream)
            };

            await stream.DisposeAsync();

            return image;
        }

        async Task<IEnumerable<Image>> SaveImageWithMultipleResolutions(string fullFileName, Func<Stream> openReadStream)
        {
            var tasks = new List<Task<Image>>();
            using (var stream = openReadStream())
            {
                foreach (var resolution in db.ImageResolutions)
                {
                    var memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    tasks.Add(SaveImageFile(fullFileName, memoryStream, resolution));
                    stream.Position = 0;
                }
            }

            return await Task.WhenAll(tasks);
        }

        public async Task DeleteImageGroup(long id)
        {
            var imageGroup = await db.ImageGroups.SingleOrDefaultAsync(i => i.Id == id) 
                ?? throw new NotFoundException<ImageGroup>(id);

            using var transaction = new TransactionScope();
            await Task.WhenAll(imageGroup.ImagesNavigation.Select(i => objectStorage.Delete(i.FileName)));

            await db.Delete<ImageGroup>(id);
        }
    }
}
