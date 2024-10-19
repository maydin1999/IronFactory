using System;
using System.Security.Cryptography;
using System.Text;

public static class Helper
{
    public static string HashPassword(string password, out string salt)
    {
        salt = GenerateSalt();
        using (var sha256 = SHA256.Create())
        {
            var combinedPassword = password + salt;
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));
            return Convert.ToBase64String(hashBytes);
        }
    }

    private static string GenerateSalt()
    {
        byte[] saltBytes = new byte[16]; // 128 bit
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }

    public static bool VerifyPassword(string password, string hashedPassword, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            var combinedPassword = password + salt;
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));
            var computedHash = Convert.ToBase64String(hashBytes);
            return computedHash == hashedPassword;
        }
    }
}
