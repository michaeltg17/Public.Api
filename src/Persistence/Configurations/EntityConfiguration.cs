using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Scaffold.Configurations
{
    public abstract class EntityConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> entity)
        {
            entity.Property(e => e.Guid).HasDefaultValueSql("newid()");
            entity.Property(e => e.CreatedOn).HasPrecision(0);
            entity.Property(e => e.ModifiedOn).HasPrecision(0);
        }
    }
}
