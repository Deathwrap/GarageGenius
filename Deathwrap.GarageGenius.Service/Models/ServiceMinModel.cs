namespace Deathwrap.GarageGenius.Service.Models;

public class ServiceMinModel
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public Decimal ExecutionTime { get; set; }
    public Decimal StandardHourPrice { get; set; }
}