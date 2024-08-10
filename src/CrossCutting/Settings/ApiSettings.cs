namespace CrossCutting.Settings
{
    public class ApiSettings : IApiSettings
    {
        /// <summary>
        /// Section or prefix for the api settings. In JSON parent level, in ENV Api__X
        /// </summary>
        public const string SectionOrPrefix = "Api";

        public string Url { get; set; } = default!;

        /// <summary>
        /// Path where images will be stored. Directory will be created if not exists.
        /// </summary>
        public string ImagesStoragePath { get; set; } = default!;
        public string ImagesRequestPath { get; set; } = default!;
        public string SqlServerConnectionString { get; set; } = default!;

        public string ImagesUrl => Flurl.Url.Combine(Url, ImagesRequestPath);
    }
}
