using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HWID.SOURCE.HELPER
{
    public class HwidHasher
    {
        public string ComputeOverall(Dictionary<string, string> parts) =>
            Sha256(string.Concat(parts.OrderBy(kv => kv.Key)
                                      .Select(kv => kv.Value ?? "")));

        public string Sha256(string input)
        {
            var sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input ?? ""));
            var sb = new StringBuilder();
            foreach (byte b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
