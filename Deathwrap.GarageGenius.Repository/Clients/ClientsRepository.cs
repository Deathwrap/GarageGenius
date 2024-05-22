using Deathwrap.GarageGenius.Data.DataAccess;
using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Repository.Clients;

public class ClientsRepository: IClientsRepository
{
    private readonly IDataAccess _db;
    public ClientsRepository(IDataAccess db)
    {
        _db= db;
    }

    public async Task<ClientConfirmed?> FindByEmail(string email)
    {
        var query = "select * from clients_confirmed where email=@Email";
        var clients=  await _db.GetData<ClientConfirmed, dynamic>(query, new { Email = email });
        return clients.FirstOrDefault();
    }

    /*public async Task UpdateRefreshToken(Client client)
    {
        var query =
            @"update clients 
                set refresh_token = @RefreshToken, refresh_token_expiry_time = @RefreshTokenExpiryTime
                where id = @Id";
        await _db.SaveData(query, new
        {
            RefreshToken = client.RefreshToken,
            RefreshTokenExpiryTime = client.RefreshTokenExpiryTime,
            Id = client.Id,
        });
    }*/

    public async Task AddClientToConfirm(ClientToConfirm client)
    {
        var query = @"insert into clients_to_confrim (id, name, email, pass_hash, creation_date, code)
                        values (@Id, @Name, @Email, @Passhash, @CreationDate, @Code)";

        await _db.SaveData(query, new {Id = client.Id, Name = client.Name, Email = client.Email, Code = client.Code, Passhash = client.PassHash, CreationDate = client.CreationDate});
    }

    public async Task<ClientToConfirm?> FindFromСlientsToConfirm(string email)
    {
        var query = @"select * from clients_to_confrim where email = @Email";

        return (await _db.GetData<ClientToConfirm, dynamic>(query, new { Email = email })).FirstOrDefault();
    }

    /*public async Task<Client?> FindFromClients(string email)
    {
        var query = @"select * from clients where email = @Email";
        return (await _db.GetData<Client, dynamic>(query, new { Email = email })).FirstOrDefault();
    }*/

    public async Task AddClientConfirmed(ClientConfirmed client)
    {
        await using var transaction = await _db.GetTransaction();

        var query = @"insert into clients_confirmed (id, name, email, pass_hash) values (@Id, @Name, @Email, @Passhash)";

        await _db.SaveData(query, new { Id = client.Id, Name = client.Name, Email = client.Email, Passhash = client.PassHash }, transaction);

        query = @"delete from clients_to_confrim where email = @Email";

        await _db.SaveData(query, new { Email = client.Email });

        await transaction.CommitAsync();
    }
}