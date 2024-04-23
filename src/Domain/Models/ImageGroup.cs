using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class ImageGroup : Entity
    {
        public string Name { get; set; } = default!;
        public long Type { get; set; }

        public virtual ImageType TypeNavigation { get; set; } = default!;

        [JsonPropertyName("Images")]
        public virtual IEnumerable<Image> ImagesNavigation { get; set; } = default!;
    }
}
