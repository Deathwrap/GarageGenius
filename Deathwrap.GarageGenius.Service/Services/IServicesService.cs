using Deathwrap.GarageGenius.Data.Models;
using Deathwrap.GarageGenius.Service.Models;

namespace Deathwrap.GarageGenius.Service.Services;

public interface IServicesService
{
    Task<IEnumerable<ServiceCategory>> GetServiceCategories();
    Task<IEnumerable<Data.Models.Service>> GetServicesByCategoryId(int categoryId);
    Task<int> AddCategory(string name);
    Task<int> AddService(ServiceMinModel serviceMinModel);
}