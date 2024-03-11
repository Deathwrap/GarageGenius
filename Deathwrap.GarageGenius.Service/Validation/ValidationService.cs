using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace Deathwrap.GarageGenius.Service.Validation;

public class ValidationService: IValidationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ValidationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<string> GenerateCode()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<string> GenerateUrl(string email, string code)
    {
        var uri = new UriBuilder
        {
            Scheme = _httpContextAccessor.HttpContext.Request.Scheme,
            Host = _httpContextAccessor.HttpContext.Request.Host.Host,
            Path = "api/confirm_email",
        };
        if (_httpContextAccessor.HttpContext.Request.Host.Port != null)
        {
            uri.Port = _httpContextAccessor.HttpContext.Request.Host.Port.Value;
        }

        uri.Query = $"email={System.Net.WebUtility.UrlEncode(email)}&code={System.Net.WebUtility.UrlEncode(code)}";

        return uri.ToString();
    }
}