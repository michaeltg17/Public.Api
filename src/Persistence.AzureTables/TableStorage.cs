using Application;
using Domain.Models;

namespace Persistence.AzureTables
{
    public class TableStorage : ITableStorage
    {
        readonly ISettings settings;

        public TableStorage(ISettings settings)
        {
            this.settings = settings;
        }

        public Task RegisterImage(Image image)
        {
            throw new NotImplementedException();
        }
    }
}