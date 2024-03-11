namespace Deathwrap.GarageGenius.Data.Models;

public class Service
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public Decimal ExecutionTime { get; set; }
    public Decimal StandardHourPrice { get; set; }
}