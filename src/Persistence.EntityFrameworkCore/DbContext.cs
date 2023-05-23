using Application;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistance.EntityFrameworkCore
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Image> Images => Set<Image>();
        public DbSet<ImageGroup> ImageGroups => Set<ImageGroup>();

        readonly ISettings settings;

        public DbContext(ISettings settings)
        {
            this.settings = settings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(settings.SqlServerConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Image>()
                .HasKey(e => e.Id);
            builder
                .Entity<Image>()
                .HasOne(i => i.Group)
                .WithMany(i => i.Images)
                .HasForeignKey(i => i.GroupId);

            builder
                .Entity<ImageGroup>()
                .HasKey(e => e.Id);
            builder
                .Entity<ImageGroup>()
                .HasMany(i => i.Images)
                .WithOne(i => i.Group)
                .HasForeignKey(i => i.GroupId);

            base.OnModelCreating(builder);
        }
    }
}
