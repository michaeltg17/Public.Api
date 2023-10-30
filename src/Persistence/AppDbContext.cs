using Application;
using Domain.Models;
using Michael.Net.Domain;
using Michael.Net.Persistance.EntityFrameworkCore.Interceptors;
using Michael.Net.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<ImageGroup> ImageGroups { get; set; }
        public virtual DbSet<ImageResolution> ImageResolutions { get; set; }
        public virtual DbSet<User> Users { get; set; }

        readonly ISettings settings;

        public AppDbContext(ISettings settings)
        {
            this.settings = settings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(settings.SqlServerConnectionString)
                .AddInterceptors(new SetAuditInfoSaveChangesInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(builder);
        }

        public Task<T> Get<T>(IQuery<T> query) => query.Execute(Database.GetDbConnection());

        public void Remove<T>(int id) where T : class, IIdentifiable, new()
        {
            var entity = new T() { Id = id };
            Remove(entity);
        }
    }
}
