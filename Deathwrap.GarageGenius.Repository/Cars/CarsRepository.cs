using Deathwrap.GarageGenius.Data.DataAccess;
using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Repository.Cars;

public class CarsRepository : ICarsRepository
{
    private readonly IDataAccess _db;
    public CarsRepository(IDataAccess db)
    {
        _db= db;
    }
    public async Task Create(Car car)
    {
        var query = @"insert into cars (id, client_id, brand, model, year, registration_number, vin) 
                        values (@Id, @ClientId, @Brand, @Model, @Year, @RegistrationNumber, @Vin)";

        await _db.SaveData(query,
            new
            {
                Id = car.Id,
                ClientId = car.ClientId,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                RegistrationNumber = car.RegistrationNumber,
                Vin = car.Vin
            });
    }

    public async Task<int> GetNextId()
    {
        var query = @"select nextval('cars_id_seq')";

        return (await _db.GetData<int, dynamic>(query, new { })).FirstOrDefault();
    }

    public async Task<IEnumerable<Car>> FindByClientId(Guid clientId)
    {
        var query = @"select * from cars where client_id = @ClientId";

        return await _db.GetData<Car, dynamic>(query, new { ClientId = clientId });
        throw new NotImplementedException();
    }
}