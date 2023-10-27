using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Models.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> entity)
        {
            entity.Property(e => e.CreatedOn).HasPrecision(0);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Guid).HasDefaultValueSql("newid()");
            entity.Property(e => e.ModifiedOn).HasPrecision(0);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Users");
        }
    }
}
