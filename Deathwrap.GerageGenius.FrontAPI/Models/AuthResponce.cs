namespace Deathwrap.GerageGenius.FrontAPI.Models;

public class AuthResponse
{
    public string Name { get; set; }
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}