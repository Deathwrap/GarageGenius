using System.Security.Cryptography;
using System.Text;
using Deathwrap.GarageGenius.Data.Models;
using Deathwrap.GarageGenius.Repository.Clients;
using Deathwrap.GarageGenius.Repository.RefreshTokens;

namespace Deathwrap.GarageGenius.Service.Clients;

public class ClientsService: IClientsService
{
    private readonly IClientsRepository _clientsRepository;
    private readonly IRefreshTokensRepository _refreshTokensRepository;

    public ClientsService(IClientsRepository clientsRepository, IRefreshTokensRepository refreshTokensRepository)
    {
        _clientsRepository = clientsRepository;
        _refreshTokensRepository = refreshTokensRepository;
    }
    public async Task AddClientForVerification(string requestName, string requestEmail, string requestPassword, string code)
    {
        var clientToConfirm = new ClientToConfirm()
        {
            Id = Guid.NewGuid(),
            Code = code,
            CreationDate = DateTime.UtcNow,
            Email = requestEmail,
            Name = requestName,
            PassHash = HashPassword(requestPassword, requestEmail),
        };
        await _clientsRepository.AddClientToConfirm(clientToConfirm);
    }

    public async Task<ClientConfirmed> GetAndCheckClient(string requestEmail, string requestPassword)
    {
        var client = await _clientsRepository.FindByEmail(requestEmail);
        if (client == null)
        {
            return null;
        }
        var hashedEnteredPassword = HashPassword(requestPassword, requestEmail);
        if (!client.PassHash.Equals(hashedEnteredPassword))
        {
            return null;
        }

        return client;
    }

    public async Task ValidateEmail(string email, string code)
    {
        var clientToValidate = await _clientsRepository.FindFromСlientsToConfirm(email);

        if (clientToValidate.Code == code)
        {
            var client = new ClientConfirmed();
            client.Id = Guid.NewGuid();
            client.Email = email;
            client.PassHash = clientToValidate.PassHash;

            await _clientsRepository.AddClientConfirmed(client);
        }
    }
    
    public static string HashPassword(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hashedBytes = sha256.ComputeHash(passwordBytes);

            return Convert.ToBase64String(hashedBytes);
        }
    }
}