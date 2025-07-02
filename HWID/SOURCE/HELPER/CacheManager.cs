using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HWID.SOURCE.HELPER
{
    public class CacheManager
    {
        private const string _Path = @"C:\ProgramData\_hwid";
        private readonly string _path;

        public CacheManager(string path = _Path) => _path = path;


        public string ResolveHwid(
            string computedHwid,
            Dictionary<string, string> currentParts,
            int Threshold = 2) // если 2 компонента из 3 идентичны по идентификаторам и хэшам, не генерируем HWID заново. (крч тоже самое железо)
        {
            if (!Load(out var savedHwid, out var savedParts))
            {
                Save(computedHwid, currentParts);
                return computedHwid;
            }

            int matches = currentParts.Keys.Count(
                k => savedParts.TryGetValue(k, out var v) && v == currentParts[k]);

            if (matches >= Threshold)
                return savedHwid;

            // новое «железо» — перезаписываем кэш
            Save(computedHwid, currentParts);
            return computedHwid;
        }


        private bool Load(out string hwid, out Dictionary<string, string> parts)
        {
            hwid = null;
            parts = null;

            if (!File.Exists(_path))
                return false;

            try
            {
                var lines = File.ReadAllLines(_path, Encoding.UTF8);
                if (lines.Length == 0)
                    return false;

                hwid = lines[0];

                parts = lines
                        .Skip(1)
                        .Select(l => l.Split(new[] { '=' }, 2))
                        .Where(a => a.Length == 2)
                        .ToDictionary(a => a[0], a => a[1]);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Save(string hwid, Dictionary<string, string> parts)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_path) ?? ".");

                var sw = new StreamWriter(_path, false, Encoding.UTF8)
                {
                    NewLine = Environment.NewLine
                };

                sw.WriteLine(hwid);
                foreach (var kv in parts)
                    sw.WriteLine($"{kv.Key}={kv.Value}");
            }
            catch
            {}
        }
    }
}
