namespace CrossCutting
{
    public class Settings : ISettings
    {
        /// <summary>
        /// Section or prefix for the api settings. In JSON parent level, in ENV Api__X
        /// </summary>
        public const string SectionOrPrefix = "Api";

        /// <summary>
        /// Path where images will be stored. Directory will be created if not exists.
        /// </summary>
        public string ImagesPath { get; set; } = default!;
        public string ImagesRequestPath { get; set; } = default!;
        public string SqlServerConnectionString { get; set; } = default!;
    }
}
