namespace Common.Testing.Models
{
    public class ImageGroup : Entity
    {
        public string Name { get; set; } = default!;
        public IEnumerable<Image> Images { get; set; } = default!;
    }
}
