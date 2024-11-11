namespace Application.Models.Responses
{
    public record File
    {
        public required byte[] Content { get; set; }
        public required string ContentType { get; set; }
        /// <summary>
        /// Full file name with extension
        /// </summary>
        public required string Name { get; set; }
    }
}
