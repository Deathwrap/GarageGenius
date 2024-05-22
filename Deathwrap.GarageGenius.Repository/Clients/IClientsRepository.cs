using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Repository.Clients;

public interface IClientsRepository
{
    //public Task Add(Client client);
    public Task<ClientConfirmed?> FindByEmail(string email);

    //public Task UpdateRefreshToken(Client client);
    Task AddClientToConfirm(ClientToConfirm client);
    Task<ClientToConfirm?> FindFromСlientsToConfirm(string email);
    //Task<Client?> FindFromClients(string email);
    Task AddClientConfirmed(ClientConfirmed client);
}