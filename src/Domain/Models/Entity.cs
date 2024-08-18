using Core.Domain;   

namespace Domain.Models
{
    public abstract class Entity : IIdentifiable, IGloballyIdentifiable, IAudited, ITestable
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsTest { get; set; } = false;
    }
}
