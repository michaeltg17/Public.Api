using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Models.Configurations
{
    public class ImagesConfiguration : IEntityTypeConfiguration<Images>
    {
        public void Configure(EntityTypeBuilder<Images> entity)
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedOn).HasPrecision(0);
            entity.Property(e => e.Guid).HasDefaultValueSql("newid()");
            entity.Property(e => e.ModifiedOn).HasPrecision(0);
            entity.Property(e => e.Url).HasMaxLength(250);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Images)
                .HasForeignKey<Images>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Images_ImageGroups");
        }
    }
}
