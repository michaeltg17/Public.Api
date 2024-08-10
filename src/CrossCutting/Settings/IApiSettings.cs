namespace CrossCutting.Settings
{
    public interface IApiSettings
    {
        public string Url { get; }
        public string ImagesStoragePath { get; }
        public string ImagesRequestPath { get; }
        public string ImagesUrl { get; }
        public string SqlServerConnectionString { get; }
    }
}
