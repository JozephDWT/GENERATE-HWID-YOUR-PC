using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWID.SOURCE.HELPER
{
    public class CacheManager
    {
        private const string GetPath = @"C:\ProgramData\_hwid";

        private readonly string _path;

        public CacheManager(string path = GetPath) => _path = path;


        public string ResolveHwid(
            string computedHwid,
            Dictionary<string, string> currentParts,
            int matchThreshold)
        {
            if (!Load(out string savedHwid, out Dictionary<string, string> savedParts))
            {
                Save(computedHwid, currentParts);
                return computedHwid;
            }

            int matches = currentParts.Keys
                                      .Count(k => savedParts.TryGetValue(k, out var v) &&
                                                  v == currentParts[k]);

            if (matches >= matchThreshold) return savedHwid;

            Save(computedHwid, currentParts);   // новое «железо»
            return computedHwid;
        }

        // ---------- внутренние методы ---------------------------------

        private bool Load(out string hwid, out Dictionary<string, string> parts)
        {
            hwid = null; parts = null;
            if (!File.Exists(_path)) return false;

            try
            {
                var lines = File.ReadAllLines(_path, Encoding.UTF8);
                if (lines.Length == 0) return false;

                hwid = lines[0];
                parts = lines.Skip(1)
                             .Select(l => l.Split(new[] { '=' }, 2))
                             .Where(a => a.Length == 2)
                             .ToDictionary(a => a[0], a => a[1]);
                return true;
            }
            catch
            {
                return false; // повреждённый кеш игнорируем
            }
        }

        private void Save(string hwid, Dictionary<string, string> parts)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_path) ?? ".");
                var sw = new StreamWriter(_path, false, Encoding.UTF8);
                sw.WriteLine(hwid);
                foreach (var kv in parts)
                    sw.WriteLine($"{kv.Key}={kv.Value}");
            }
            catch { /* недостаточно прав? игнор */ }
        }
    }
}
