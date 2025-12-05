using System.Security.Cryptography;
using System.Text;

namespace DepDB.Helpers;

public class PasswordHasher
{
    public string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        return HashPassword(hashedPassword) == providedPassword;
    }
}