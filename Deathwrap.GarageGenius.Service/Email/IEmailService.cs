namespace Deathwrap.GarageGenius.Service.Email;

public interface IEmailService
{
    Task Send(string url, string requestEmail);
}