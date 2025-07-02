using System;
using System.Security.Cryptography;
using System.Text;

namespace HWID.SOURCE.HELPER.Hasher
{
    //помощник SHA-256

    internal static class SerialHasher
    {
        private const string Unknown = "Unknown";

        public static string ComputeSha256(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Unknown;

            var sha = SHA256.Create();
            byte[] digest = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
            return BitConverter.ToString(digest).Replace("-", string.Empty);
        }
    }
}
