using System.ComponentModel.DataAnnotations;

namespace Application
{
    public class Settings : ISettings
    {
        [Required]
        public string AzureStorageConnectionString { get; set; } = default!;

        [Required]
        public string ImagesPath { get; set; } = default!;

        [Required]
        public string ImagesRequestPath { get; set; } = default!;

        [Required]
        public string SqlServerConnectionString { get; set; } = default!;
    }
}
