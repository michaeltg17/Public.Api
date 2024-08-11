namespace CrossCutting.Settings
{
    public class ApiSettings : IApiSettings
    {
        /// <summary>
        /// Section or prefix for the api settings. In JSON parent level, in ENV Api__X
        /// </summary>
        public const string SectionOrPrefix = "Api";

        public required string Url { get; set; }

        /// <summary>
        /// Path where images will be stored. Directory will be created if not exists.
        /// </summary>
        public required string ImagesStoragePath { get; set; }
        public required string ImagesRequestPath { get; set; }
        public required string SqlServerConnectionString { get; set; }

        public string ImagesUrl => Flurl.Url.Combine(Url, ImagesRequestPath);
    }
}
