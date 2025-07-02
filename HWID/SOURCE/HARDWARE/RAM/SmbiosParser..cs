using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HWID.SOURCE.HARDWARE
{
    // Мини-парсер SMBIOS.
    // Ищет записи типа 17 (Memory Device) и вытягивает Serial Number
   
    internal static class SmbiosParser
    {
        public static IReadOnlyCollection<string> ExtractRamSerials(byte[] data)
        {
            var serials = new List<string>();

            if (data == null || data.Length < 8)
                return serials;

            int a = BitConverter.ToInt32(data, 4);
            int b = 8;
            int c = Math.Min(b + a, data.Length);

            while (b + 4 <= c)
            {
                byte type = data[b];
                byte length = data[b + 1];
                if (length == 0 || b + length > c)
                    break;

                if (type == 17 && length > 0x18)
                {
                    byte idx = data[b + 0x18];
                    if (idx != 0)
                    {
                        string sn = GetString.Get(data, b + length, c, idx); // отдельный класс GetString.Get
                        if (!string.IsNullOrWhiteSpace(sn))
                            serials.Add(sn.Trim());
                    }
                }

                int next = b + length;
                while (next + 1 < c && (data[next] != 0 || data[next + 1] != 0))
                    next++;

                next += 2;
                b = next;
            }

            return serials
                   .Distinct(StringComparer.OrdinalIgnoreCase)
                   .OrderBy(s => s, StringComparer.OrdinalIgnoreCase)
                   .ToArray();
        }
    }
}
