using System;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace Felicity.API.Services
{
    public class SecurityService
    {
        static readonly RNGCryptoServiceProvider RNGCryptoService = new RNGCryptoServiceProvider();

        public static JObject EncodePassword(string password, byte[] salt = null)
        {
            if (string.IsNullOrEmpty(password))
                return new JObject {
                    "Please enter a valid password"
                };

            if (salt == null)
                new RNGCryptoServiceProvider().GetNonZeroBytes(salt = new byte[16]);

            var hasher = new Rfc2898DeriveBytes(password, salt, 10000);

            byte[] hash = hasher.GetBytes(20);
            byte[] hashBytes = new byte[36];

            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordSalt = Convert.ToBase64String(salt);
            string savedPasswordHash = Convert.ToBase64String(hashBytes);

            JObject credentials = new JObject
            {
                {"salt", savedPasswordSalt },
                {"hash", savedPasswordHash }
            };

            return credentials;
        }

        public static bool ValidatePassword(string hash, string salt, string password)
        {
            byte[] hashBytes = Convert.FromBase64String(hash);
            byte[] saltBytes = new byte[16];

            Array.Copy(hashBytes, 0, saltBytes, 0, 16);

            var hasher = new Rfc2898DeriveBytes(password, saltBytes, 10000);

            byte[] newHash = hasher.GetBytes(20);

            bool valid = true;

            for (int i = 0; i < 20; i++) {
                if (hashBytes[i + 16] != newHash[i])
                    valid = false;
            }

            return valid;
        }
    }
}
