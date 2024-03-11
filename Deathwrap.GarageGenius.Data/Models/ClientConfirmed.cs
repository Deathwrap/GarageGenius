namespace Deathwrap.GarageGenius.Data.Models;

public class ClientConfirmed
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public string PassHash { get; set; }
}