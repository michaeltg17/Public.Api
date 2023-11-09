namespace CrossCutting
{
    public interface ISettings
    {
        public string ImagesPath { get; }
        public string ImagesRequestPath { get; }
        public string SqlServerConnectionString { get; }
    }
}
