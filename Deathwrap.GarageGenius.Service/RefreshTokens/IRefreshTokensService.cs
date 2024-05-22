using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Service.RefreshTokens;

public interface IRefreshTokensService
{
    Task AddRefreshToken(Guid userId, Guid sessionId, string refreshToken);
    Task<RefreshToken> GetRefreshToken(Guid userId, Guid sessionId);
    Task UpdateRefreshToken(RefreshToken oldRefreshToken, string newRefreshToken);
    Task DeleteRefreshToken(Guid userId, Guid sessionId);
}