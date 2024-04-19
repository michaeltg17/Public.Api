using CrossCutting;
using Domain.Models;
using Michael.Net.Domain;
using Michael.Net.Persistance.EntityFrameworkCore.Interceptors;
using Michael.Net.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options, ISettings settings) : DbContext(options)
    {
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<ImageGroup> ImageGroups { get; set; }
        public virtual DbSet<ImageType> ImageTypes { get; set; }
        public virtual DbSet<ImageResolution> ImageResolutions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ImageFileExtension> ImageFileExtensions { get; set; }

        readonly ISettings settings = settings;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options
                .UseSqlServer(settings.SqlServerConnectionString)
                .AddInterceptors(new SetAuditInfoSaveChangesInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(builder);
        }

        public Task<T> Get<T>(IQuery<T> query) => query.Execute(Database.GetDbConnection());

        public Task<int> Delete<T>(long id) where T : class, IIdentifiable, new()
        {
            return Set<T>().Where(e => e.Id == id).ExecuteDeleteAsync();
        }
    }
}
