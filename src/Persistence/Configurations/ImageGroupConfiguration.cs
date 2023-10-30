using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Scaffold.Configurations
{
    public class ImageGroupConfiguration : EntityConfiguration<ImageGroup>
    {
        public override void Configure(EntityTypeBuilder<ImageGroup> entity)
        {
            base.Configure(entity);
        }
    }
}
