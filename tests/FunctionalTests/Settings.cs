using System.ComponentModel.DataAnnotations;

namespace FunctionalTests
{
    public class Settings : ISettings
    {
        /// <summary>
        /// Section or prefix for the settings. In JSON parent level, in ENV FunctionalTests__X
        /// </summary>
        public const string SectionOrPrefix = "FunctionalTests";

        [Required]
        public string ApiUrl { get; set; } = default!;
    }
}
