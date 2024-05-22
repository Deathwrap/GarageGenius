using Deathwrap.GarageGenius.Data.DataAccess;
using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Repository.RefreshTokens;

public class RefreshTokensRepository(IDataAccess db) : IRefreshTokensRepository
{
    private readonly IDataAccess _db = db;

    public async Task<int> GetNextId()
    {
        var query = @"select nextval('refresh_tokens_id_seq')";
        return (await _db.GetData<int, dynamic>(query, new { })).FirstOrDefault();
    }

    public async Task Create(RefreshToken refreshToken)
    {
        var query = @"insert into refresh_tokens (id, token_owner_id, session_id, token, creation_date) 
                        values (@Id, @TokenOwnerId, @SessionId, @Token, @CreationDate)";
        await _db.SaveData(query,
            new
            {
                Id = refreshToken.Id,
                TokenOwnerId = refreshToken.TokenOwnerId,
                SessionId = refreshToken.SessionId,
                Token = refreshToken.Token,
                CreationDate = refreshToken.CreationDate
            });
    }

    public async Task Update(RefreshToken refreshToken)
    {
        var query = @"update refresh_tokens set (creation_date, token) = (@CreationDate, @Token) where id = @Id";
        await _db.SaveData(query, new
        {
            CreationDate = refreshToken.CreationDate,
            Token = refreshToken.Token,
            Id = refreshToken.Id
        });
    }

    public async Task<RefreshToken?> FindByOwnerIdAndSessionId(Guid ownerId, Guid sessionId)
    {
        var query = @"select * from refresh_tokens where token_owner_id = @OwnerId and session_id = @SessionId";

        return (await _db.GetData<RefreshToken, dynamic>(query, new { OwnerId = ownerId, SessionId = sessionId }))
            .FirstOrDefault();
    }

    public async Task Delete(RefreshToken? refreshToken)
    {
        var query = @"delete from refresh_tokens where id = @Id";
        await _db.SaveData(query, new { Id = refreshToken.Id });
    }
}