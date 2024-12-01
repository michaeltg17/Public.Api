using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public abstract class EntityConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> entity)
        {
            entity.Property(e => e.Guid).HasDefaultValueSql("NEWID()");
            entity.HasQueryFilter(e => e.IsTest == false);
        }
    }
}
