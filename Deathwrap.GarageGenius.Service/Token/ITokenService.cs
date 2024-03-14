using System.Security.Claims;
using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Service.Token;

public interface ITokenService
{
    string CreateClientToken(ClientConfirmed client, Guid sessionId);
    string CreateWorkerToken(Worker worker, Guid sessionId, string role);

    ClaimsPrincipal GetPrincipalFromExpiredToken(string? accessToken);
    string GenerateAccessToken(IEnumerable<Claim> principalClaims);
}