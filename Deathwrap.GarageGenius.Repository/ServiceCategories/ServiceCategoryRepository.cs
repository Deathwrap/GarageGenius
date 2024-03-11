using Deathwrap.GarageGenius.Data.DataAccess;
using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Repository.ServiceCategories;

public class ServiceCategoryRepository: IServiceCategoryRepository
{
    private readonly IDataAccess _db;
    public ServiceCategoryRepository(IDataAccess db)
    {
        _db = db;
    }
    public async Task<IEnumerable<ServiceCategory>> FindAll()
    {
        var query = @"select * from service_categories";
        return await _db.GetData<ServiceCategory, dynamic>(query, new { });
    }

    public async Task<int> GetNextId()
    {
        var query = @"select nextval('service_categories_id_seq')";

        return (await _db.GetData<int, dynamic>(query, new { })).FirstOrDefault();    
    }

    public async Task Create(ServiceCategory serviceCategory)
    {
        var query = @"insert into service_categories (id, name) values (@Id, @Name)";
        await _db.SaveData(query, new { Id = serviceCategory.Id, Name = serviceCategory.Name });
    }
}