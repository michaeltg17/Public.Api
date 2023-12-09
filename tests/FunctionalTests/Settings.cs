using System.ComponentModel.DataAnnotations;

namespace FunctionalTests
{
    public class Settings : ISettings
    {
        [Required]
        public string ApiUrl { get; set; } = default!;
    }
}
