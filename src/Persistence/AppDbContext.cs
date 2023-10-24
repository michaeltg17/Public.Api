using Application;
using Domain.Models;
using Michael.Net.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Persistance.EntityFrameworkCore
{
    public class AppDbContext : DbContext
    {
        public DbSet<Image> Images => Set<Image>();
        public DbSet<ImageGroup> ImageGroups => Set<ImageGroup>();

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
    }
}
