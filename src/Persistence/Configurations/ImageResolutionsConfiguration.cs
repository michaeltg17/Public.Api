using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Models.Configurations
{
    public class ImageResolutionsConfiguration : IEntityTypeConfiguration<ImageResolutions>
    {
        public void Configure(EntityTypeBuilder<ImageResolutions> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK_ImageResolution");

            entity.Property(e => e.CreatedOn).HasPrecision(0);
            entity.Property(e => e.Guid).HasDefaultValueSql("newid()");
            entity.Property(e => e.ModifiedOn).HasPrecision(0);
            entity.Property(e => e.Name).HasMaxLength(25);
        }
    }
}
