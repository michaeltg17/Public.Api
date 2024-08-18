namespace Core.Persistence
{
    public interface IObjectStorage
    {
        Task<Stream?> Get(string fullFileName);
        Task<Stream> GetOrThrow(string fullFileName);
        Task<string> Upload(string fullFileName, Stream stream);
        Task Delete(string fullFileName);
    }
}
