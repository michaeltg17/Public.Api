using System.Collections.Generic;

namespace Application.Models.DataTransferObjects
{
    public class ImageGroup : Entity
    {
        public string Name { get; set; } = default!;
        public IEnumerable<Image> Images { get; set; } = default!;
    }
}
