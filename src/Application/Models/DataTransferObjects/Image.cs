namespace Application.Models.DataTransferObjects
{
    public class Image : Entity
    {
        public required string Url { get; init; }
        public long Resolution { get; init; }
        public long Group { get; init; }
        public required string FileName { get; init; }
    }
}
