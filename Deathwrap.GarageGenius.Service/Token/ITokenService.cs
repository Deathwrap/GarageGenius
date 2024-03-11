using System.Security.Claims;
using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Service.Token;

public interface ITokenService
{
    string CreateToken(ClientConfirmed client, Guid sessionId);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string? accessToken);
    string GenerateAccessToken(IEnumerable<Claim> principalClaims);
}