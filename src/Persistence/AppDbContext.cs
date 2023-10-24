using Application;
using Domain.Models;
using Michael.Net.Domain;
using Michael.Net.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Image> Images => Set<Image>();
        public DbSet<ImageGroup> ImageGroups => Set<ImageGroup>();
        public DbSet<ImageResolution> ImageResolutions => Set<ImageResolution>();

        readonly ISettings settings;

        public AppDbContext(ISettings settings)
        {
            this.settings = settings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(settings.SqlServerConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public Task<T> Get<T>(IQuery<T> query) => query.Execute(Database.GetDbConnection());

        public void Remove<T>(int id) where T : class, IEntity, new()
        {
            var entity = new T() { Id = id };
            Remove(entity);
        }
    }
}
