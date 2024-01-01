using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Scaffold.Configurations
{
    public class ImageFileExtensionConfiguration : EntityConfiguration<ImageFileExtension>
    {
        public override void Configure(EntityTypeBuilder<ImageFileExtension> entity)
        {
            base.Configure(entity);
        }
    }
}
