using Domain.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IImageService
    {
        Task<Image> GetImage(long id);
        Task<ImageGroup> GetImageGroup(long id);
        Task<ImageGroup> SaveImageGroup(string fullFileName, Func<Stream> openReadStream);
        void DeleteImageGroup(long id);
    }
}
