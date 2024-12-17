using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class ImageResolutionConfiguration : EntityConfiguration<ImageResolution>
    {
        public override void Configure(EntityTypeBuilder<ImageResolution> entity)
        {
            base.Configure(entity);
        }
    }
}
