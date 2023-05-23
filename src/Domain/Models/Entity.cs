using Dapper.Contrib.Extensions;
using Michael.Net.Domain;

namespace Domain.Models
{
    public abstract class Entity : IEntity
    {
        public long Id { get; set; }
        [Write(false)] public Guid Guid { get; set; }
    }
}
