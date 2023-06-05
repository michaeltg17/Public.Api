using Dapper.Contrib.Extensions;
using Michael.Net.Domain;

namespace Domain.Models
{
    public abstract class Entity : IEntity, IAudited
    {
        public long Id { get; set; }
        [Write(false)] public Guid Guid { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
