namespace Deathwrap.GarageGenius.Data.Models;

public class Car
{
    public int Id { get; set; }
    public Guid ClientId { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string RegistrationNumber { get; set; }
    public string Vin { get; set; }
}