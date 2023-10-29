using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Scaffold.Configurations
{
    public class ImageResolutionConfiguration : EntityConfiguration<ImageResolution>
    {
        public override void Configure(EntityTypeBuilder<ImageResolution> entity)
        {
            base.Configure(entity);
            entity.Property(e => e.Name).HasMaxLength(25);
        }
    }
}
