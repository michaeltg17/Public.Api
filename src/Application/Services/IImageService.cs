using Domain.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IImageService
    {
        Task<Image> GetImage(int id);
        Task<ImageGroup> GetImageGroup(int id);
        Task<ImageGroup> SaveImageGroup(string fullFileName, Func<Stream> openReadStream);
        void DeleteImageGroup(int id);
    }
}
