using Domain.Models;

namespace Persistence.AzureTables
{
    public interface ITableStorage
    {
        Task RegisterImage(Image image);
    }
}
