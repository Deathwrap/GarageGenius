namespace Deathwrap.GarageGenius.Data.Models;

public class ClientToConfirm
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public string PassHash { get; set; }
    public string Code { get; set; }
    public DateTime CreationDate { get; set; }
}