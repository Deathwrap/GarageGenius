using Deathwrap.GarageGenius.Data.Models;
using Deathwrap.GarageGenius.Repository.ServiceCategories;
using Deathwrap.GarageGenius.Repository.Services;
using Deathwrap.GarageGenius.Service.Models;

namespace Deathwrap.GarageGenius.Service.Services;

public class ServicesService: IServicesService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IServiceCategoryRepository _serviceCategoryRepository;

    public ServicesService(IServiceRepository serviceRepository,
        IServiceCategoryRepository serviceCategoryRepository)
    {
        _serviceRepository = serviceRepository;
        _serviceCategoryRepository = serviceCategoryRepository;
    }
    
    public async Task<IEnumerable<ServiceCategory>> GetServiceCategories()
    {
        var serviceCategories = await _serviceCategoryRepository.FindAll();
        return serviceCategories;
    }

    public async  Task<IEnumerable<Data.Models.Service>> GetServicesByCategoryId(int categoryId)
    {
        var services = await _serviceRepository.FindByCategoryId(categoryId);
        return services;
    }

    public async Task<int> AddCategory(string name)
    {
        var categoryId = await  _serviceCategoryRepository.GetNextId();

        var category = new ServiceCategory()
        {
            Id = categoryId,
            Name = name
        };

        await _serviceCategoryRepository.Create(category);

        return categoryId;
    }

    public async Task<int> AddService(ServiceMinModel serviceMin)
    {
        var serviceId = await _serviceRepository.GetNextId();

        var service = new Data.Models.Service
        {
            Id = serviceId,
            CategoryId = serviceMin.CategoryId,
            ExecutionTime = serviceMin.ExecutionTime,
            Name = serviceMin.Name,
            StandardHourPrice = serviceMin.StandardHourPrice,
        };

        await _serviceRepository.Create(service);

        return serviceId;
    }
}