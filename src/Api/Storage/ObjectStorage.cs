using ChinhDo.Transactions;
using Core.Persistence;

namespace Api.Storage
{
    public class ObjectStorage(string path, string url, IFileManager fileManager) : IObjectStorage
    {
        public Task Delete(string fullFileName)
        {
            throw new NotImplementedException();
        }

        public Task<Stream?> Get(string fullFileName)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> GetOrThrow(string fullFileName)
        {
            throw new NotImplementedException();
        }

        public Task<string> Upload(string fullFileName, Stream stream)
        {
            var filePath = Path.Combine(path, fullFileName);

            var temporalFilePath = fileManager.CreateTempFileName();
            using (var fs = new FileStream(temporalFilePath, FileMode.Create))
            {
                stream.CopyTo(fs);
            }

            fileManager.Copy(temporalFilePath, filePath, true);

            var fileUrl = $"{url}/{fullFileName}";

            return Task.FromResult(fileUrl);
        }
    }
}
