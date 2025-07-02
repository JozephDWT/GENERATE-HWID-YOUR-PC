using HWID.SOURCE.HELPER.Hasher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWID.SOURCE.HELPER
{
    public class Writer
    {
        private readonly HwidHasher _hasher;

        public Writer(HwidHasher hasher) => _hasher = hasher;

        public void WriteDesktopReport(
            string hwid, Dictionary<string, string> parts)
        {
            var sb = new StringBuilder()
                .AppendLine("===== ОТЧЕТ =====")
                .AppendLine($"Создан: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                .AppendLine()
                .AppendLine($"Финальный HWID (SHA-256): {hwid}")
                .AppendLine()
                .AppendLine("---- Сырые идентификаторы ----");

            foreach (var kv in parts)
                sb.AppendLine($"{kv.Key,-15}: {kv.Value}");

            sb.AppendLine()
              .AppendLine("---- Индивидуальные хэши SHA-256 ----");

            foreach (var kv in parts)
                sb.AppendLine($"{kv.Key,-15}: {_hasher.Sha256(kv.Value)}");

            sb.AppendLine()
              .AppendLine("HWID ОСНОВАН: " +
                          string.Join(" | ", parts.Keys.OrderBy(k => k)))
              .AppendLine("========================");

            string desktop = Environment.GetFolderPath(
                                Environment.SpecialFolder.Desktop);
            File.WriteAllText(Path.Combine(desktop, "Your_PC_Hwid.txt"),
                              sb.ToString(),
                              Encoding.UTF8);
        }
    }
}
