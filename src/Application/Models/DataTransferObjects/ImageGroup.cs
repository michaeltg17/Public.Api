namespace Application.Models.DataTransferObjects
{
    public class ImageGroup : Entity
    {
        public required string Name { get; init; }
        public required IEnumerable<Image> Images { get; init; }
    }
}
