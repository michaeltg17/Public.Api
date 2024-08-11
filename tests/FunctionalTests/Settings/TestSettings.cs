using System.ComponentModel.DataAnnotations;

namespace FunctionalTests.Settings
{
    public class TestSettings : ITestSettings
    {
        [Required]
        public required string ApiUrl { get; init; }
    }
}
