using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Scaffold.Configurations
{
    public class ImageConfiguration : EntityConfiguration<Image>
    {
        public override void Configure(EntityTypeBuilder<Image> entity)
        {
            base.Configure(entity);
            entity.Property(e => e.Url).HasMaxLength(250);

            entity
                .HasOne(d => d.ImageGroup)
                .WithMany(p => p.Images)
                .HasForeignKey(d => d.ImageGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Images_ImageGroups");
        }
    }
}
