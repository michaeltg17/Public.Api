using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class ImageGroupConfiguration : EntityConfiguration<ImageGroup>
    {
        public override void Configure(EntityTypeBuilder<ImageGroup> entity)
        {
            base.Configure(entity);

            entity
                .HasOne(e => e.TypeNavigation)
                .WithMany(e => e.ImageGroupNavigation)
                .HasForeignKey(e => e.Type);
        }
    }
}
