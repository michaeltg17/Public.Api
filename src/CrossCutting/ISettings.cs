namespace Application
{
    public interface ISettings
    {
        public string AzureStorageConnectionString { get; }
        public string ImagesPath { get; }
        public string ImagesRequestPath { get; }
        public string SqlServerConnectionString { get; }
    }
}
