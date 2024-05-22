namespace Deathwrap.GarageGenius.Data.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public Guid TokenOwnerId { get; set; }
    public Guid SessionId { get; set; }
    public string Token { get; set; }
    public DateTime CreationDate { get; set; }
}