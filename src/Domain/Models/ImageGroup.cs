using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class ImageGroup : Entity
    {
        public required string Name { get; init; }
        public long Type { get; init; }

        public virtual ImageType TypeNavigation { get; set; } = default!;

        [JsonPropertyName("Images")]
        public virtual IEnumerable<Image> ImagesNavigation { get; set; } = default!;
    }
}
