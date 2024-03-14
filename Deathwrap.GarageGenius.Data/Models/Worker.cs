namespace Deathwrap.GarageGenius.Data.Models;

public class Worker
{
    public Guid Id { get; set; }
    public int PositionId { get; set; }
    public string Name { get; set; }
    public string Login { get; set; }
    public string PassHash { get; set; }
}
