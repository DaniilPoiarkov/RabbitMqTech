using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace RabbitMq.Identity.AuthServices
{
    internal sealed class AuthHelper
    {
        public static string HashPassword(string password, byte[] salt) =>
            Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            )
        );

        internal static string HashPassword(string password, string salt) =>
            HashPassword(password, Convert.FromBase64String(salt));

        internal static byte[] GetBytes(int length = 32) =>
            RandomNumberGenerator.GetBytes(length);

        internal static bool ValidatePassword(string password, string salt, string hash) =>
            HashPassword(password, salt) == hash;
    }
}
