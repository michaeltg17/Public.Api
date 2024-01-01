using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Scaffold.Configurations
{
    public class ImageConfiguration : EntityConfiguration<Image>
    {
        public override void Configure(EntityTypeBuilder<Image> entity)
        {
            base.Configure(entity);

            entity
                .HasOne(e => e.GroupNavigation)
                .WithMany(e => e.ImagesNavigation)
                .HasForeignKey(e => e.Group);

            entity
                .HasOne(e => e.ResolutionNavigation)
                .WithMany(e => e.ImagesNavigation)
                .HasForeignKey(e => e.Resolution);
        }
    }
}
