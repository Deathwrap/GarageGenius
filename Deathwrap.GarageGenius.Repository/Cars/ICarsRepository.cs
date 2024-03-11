using Deathwrap.GarageGenius.Data.DataAccess;
using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Repository.Cars;

public interface ICarsRepository
{

    Task Create(Car car);
    Task<int> GetNextId();
    Task<IEnumerable<Car>> FindByClientId(Guid clientId);
}