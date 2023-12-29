using Michael.Net.Extensions;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TestService(AppDbContext db)
    {
        public async Task DeleteAllTestEntities()
        {
            var sets = db
                .GetType()
                .GetProperties()
                .Where(p => p.GetType() == typeof(DbSet<>));

            var method = db.GetType().GetMethod("Set");

            //db.Set().Where(e => e.Id == id).ExecuteDeleteAsync();
        }
    }
}
