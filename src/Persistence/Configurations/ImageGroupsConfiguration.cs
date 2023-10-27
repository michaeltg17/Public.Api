using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Models.Configurations
{
    public class ImageGroupsConfiguration : IEntityTypeConfiguration<ImageGroups>
    {
        public void Configure(EntityTypeBuilder<ImageGroups> entity)
        {
            entity.Property(e => e.CreatedOn).HasPrecision(0);
            entity.Property(e => e.Guid).HasDefaultValueSql("newid()");
            entity.Property(e => e.ModifiedOn).HasPrecision(0);
        }
    }
}
