using Deathwrap.GarageGenius.Data.Models;
using Deathwrap.GarageGenius.Repository.Cars;
using Deathwrap.GarageGenius.Service.Models;

namespace Deathwrap.GarageGenius.Service.Cars;

public class CarsService: ICarsService
{
    private readonly ICarsRepository _carsRepository;

    public CarsService(ICarsRepository carsRepository)
    {
        _carsRepository = carsRepository;
    }
    public async Task<IEnumerable<Car>> GetCarsByClientId(Guid clientId)
    {
        var cars = await _carsRepository.FindByClientId(clientId);
        return cars;
    }

    public async Task<int> Add(CarMinModel car, Guid clientId)
    {
        var carId = await _carsRepository.GetNextId();
        var newCar = new Car()
        {
            Id = carId,
            ClientId = clientId,
            Brand = car.Brand,
            Model = car.Model,
            RegistrationNumber = car.RegistrationNumber,
            Vin = car.Vin,
            Year = car.Year,
        };

        await _carsRepository.Create(newCar);

        return carId;
    }
}