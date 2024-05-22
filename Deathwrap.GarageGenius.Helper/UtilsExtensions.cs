using System.Security.Cryptography;
using System.Text;

namespace Deathwrap.GarageGenius.Helper;

public class UtilsExtensions
{
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