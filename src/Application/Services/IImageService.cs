using Domain.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IImageService
    {
        Task<Image> GetImage(long id, CancellationToken token);
        Task<ImageGroup> GetImageGroup(long id, CancellationToken token);
        Task<ImageGroup> SaveImageGroup(string fullFileName, Func<Stream> openReadStream);
        Task DeleteImageGroup(long id);
    }
}
