using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Services
{
    public class TestService(AppDbContext db)
    {
        public Task DeleteAllTestEntities()
        {
            var sets = db
                .GetType()
                .GetProperties()
                .Where(p => p.GetType() == typeof(DbSet<>));

            var method = db.GetType().GetMethod("Set");

            //db.Set().Where(e => e.Id == id).ExecuteDeleteAsync();

            return Task.CompletedTask;
        }
    }
}
