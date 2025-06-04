using System.Security.Cryptography;
using System.Text;

namespace OrderService.Services;

public static class Hasher
{
    public static string GenerateSalt(int length = 16)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(length);
        return Convert.ToBase64String(salt);
    }

    public static string HashPassword(string password, string salt)
    {
        string combined = password + salt;
        using var sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
        return Convert.ToBase64String(hash);
    }
}