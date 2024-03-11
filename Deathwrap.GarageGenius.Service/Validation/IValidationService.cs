namespace Deathwrap.GarageGenius.Service.Validation;

public interface IValidationService
{
    Task<string> GenerateCode();
    Task<string> GenerateUrl(string email, string code);
}