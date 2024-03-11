using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Repository.RefreshTokens;

public interface IRefreshTokensRepository
{
    Task<int> GetNextId();
    Task Create(RefreshToken refreshToken);
    Task Update(RefreshToken refreshToken);
    public Task<RefreshToken?> FindByOwnerIdAndSessionId(Guid ownerId, Guid sessionId);
    Task Delete(RefreshToken? refreshToken);
}