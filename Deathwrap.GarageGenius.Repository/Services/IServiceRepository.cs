using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Repository.Services;

public interface IServiceRepository
{
    Task<IEnumerable<Service>> FindByCategoryId(int categoryId);
    Task<int> GetNextId();

    Task Create(Service service);
}