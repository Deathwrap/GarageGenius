namespace Deathwrap.GerageGenius.FrontAPI.Models;

public class ClientAuthRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}