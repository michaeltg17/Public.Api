using static System.Net.Mime.MediaTypeNames;

namespace Domain.Models
{
    public class ImageGroup : Entity
    {
        public string Name { get; set; } = default!;
        public IEnumerable<Image> Images { get; set; } = default!;
        public virtual Image? Images { get; set; }
    }
}
