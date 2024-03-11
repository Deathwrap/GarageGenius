using Deathwrap.GarageGenius.Data.Models;
using Deathwrap.GarageGenius.Repository.RefreshTokens;

namespace Deathwrap.GarageGenius.Service.RefreshTokens;

public class RefreshTokensService: IRefreshTokensService
{
    private readonly IRefreshTokensRepository _refreshTokensRepository;

    public RefreshTokensService(IRefreshTokensRepository refreshTokensRepository)
    {
        _refreshTokensRepository = refreshTokensRepository;
    }

    public async Task AddRefreshToken(Guid userId, Guid sessionId, string refreshToken)
    {
        var refreshTokenId = await _refreshTokensRepository.GetNextId();
        var newRefreshToken = new RefreshToken()
        {
            Id = refreshTokenId,
            CreationDate = DateTime.UtcNow,
            SessionId = sessionId,
            Token = refreshToken,
            TokenOwnerId = userId,
        };

        await _refreshTokensRepository.Create(newRefreshToken);
    }

    public async Task<RefreshToken> GetRefreshToken(Guid userId, Guid sessionId)
    {
        var refreshToken = await _refreshTokensRepository.FindByOwnerIdAndSessionId(userId, sessionId);
        return refreshToken;
    }

    public async Task UpdateRefreshToken(RefreshToken oldRefreshToken, string newRefreshToken)
    {
        var newRFToken = new RefreshToken()
        {
            Id = oldRefreshToken.Id,
            CreationDate = DateTime.UtcNow,
            SessionId = oldRefreshToken.SessionId,
            Token = newRefreshToken,
            TokenOwnerId = oldRefreshToken.TokenOwnerId,
        };

        await _refreshTokensRepository.Update(newRFToken);
    }

    public async Task DeleteRefreshToken(Guid userId, Guid sessionId)
    {
        var refreshToken = await _refreshTokensRepository.FindByOwnerIdAndSessionId(userId, sessionId);
        await _refreshTokensRepository.Delete(refreshToken);
    }
}