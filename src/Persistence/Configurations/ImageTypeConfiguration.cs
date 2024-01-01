using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Scaffold.Configurations
{
    public class ImageTypeConfiguration : EntityConfiguration<ImageType>
    {
        public override void Configure(EntityTypeBuilder<ImageType> entity)
        {
            base.Configure(entity);

            entity
                .HasMany(e => e.FileExtensionNavigation)
                .WithOne(e => e.ImageTypeNavigation)
                .HasForeignKey(e => e.ImageType);
        }
    }
}
