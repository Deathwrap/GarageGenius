using Deathwrap.GarageGenius.Data.Models;
using Deathwrap.GarageGenius.Service.Models;

namespace Deathwrap.GarageGenius.Service.Cars;

public interface ICarsService
{
    Task<IEnumerable<Car>> GetCarsByClientId(Guid clientId);
    Task<int> Add(CarMinModel car, Guid clientId);
}