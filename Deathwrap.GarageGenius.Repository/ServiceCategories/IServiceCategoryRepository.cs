using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Repository.ServiceCategories;

public interface IServiceCategoryRepository
{
    Task<IEnumerable<ServiceCategory>> FindAll();
    Task<int> GetNextId();

    Task Create(ServiceCategory serviceCategory);
}