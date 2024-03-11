using Deathwrap.GarageGenius.Data.DataAccess;
using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Repository.Services;

public class ServiceRepository: IServiceRepository
{
    private readonly IDataAccess _db;
    public ServiceRepository(IDataAccess db)
    {
        _db = db;
    }
    
    public async Task<IEnumerable<Service>> FindByCategoryId(int categoryId)
    {
        var query = @"select * from services where category_id = @CategoryId";
        return await _db.GetData<Service, dynamic>(query, new { CategoryId = categoryId });
    }
    public async Task<int> GetNextId()
    {
        var query = @"select nextval('services_id_seq')";

        return (await _db.GetData<int, dynamic>(query, new { })).FirstOrDefault();    
    }

    public async Task Create(Service service)
    {
        var query = @"insert into services (id, category_id, name, execution_time, standard_hour_price) 
                        values (@Id, 
                                @CategoryId,
                                @Name,
                                @ExecutionTime,
                                @StandardHourPrice)";
        await _db.SaveData(query, new { Id = service.Id, 
            CategoryId = service.CategoryId,
            Name = service.Name,
            ExecutionTime = service.ExecutionTime,
            StandardHourPrice = service.StandardHourPrice
        });
    }
}