using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Service.Clients;

public interface IClientsService
{
    Task AddClientForVerification(string requestName, string requestEmail, string requestPassword, string code);
    Task<ClientConfirmed> GetAndCheckClient(string requestEmail, string requestPassword);
    Task ValidateEmail(string email, string code);
}