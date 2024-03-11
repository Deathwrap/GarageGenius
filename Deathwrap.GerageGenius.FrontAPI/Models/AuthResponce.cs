namespace Deathwrap.GerageGenius.FrontAPI.Models;

public class AuthResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}