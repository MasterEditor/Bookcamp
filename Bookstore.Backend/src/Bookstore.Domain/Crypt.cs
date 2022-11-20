using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Bookstore.Domain
{
    public static class Crypt
    {
        public static string HashString(string inputKey, string salt)
        {
            if (string.IsNullOrEmpty(inputKey))
            {
                throw new ArgumentNullException(nameof(inputKey));
            }

            if (string.IsNullOrEmpty(salt))
            {
                throw new ArgumentNullException(nameof(inputKey));
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: inputKey,
            salt: Encoding.UTF8.GetBytes(salt),
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

            return hashed;
        }

        public static string GetSalt()
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            var salt = new byte[128 / 8];

            rng.GetBytes(salt);
            return BitConverter.ToString(salt);
        }

        public static bool Verify(string text, string hashed, string salt)
        {
            return SecureEquals(Encoding.UTF8.GetBytes(hashed), Encoding.UTF8.GetBytes(HashString(text, salt)));
        }

        private static bool SecureEquals(byte[] a, byte[] b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null || a.Length != b.Length)
            {
                return false;
            }

            int num = 0;
            for (int i = 0; i < a.Length; i++)
            {
                num |= a[i] ^ b[i];
            }

            return num == 0;
        }
    }
}
