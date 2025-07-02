using HWID.SOURCE.HELPER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace HWID.SOURCE.HARDWARE
{

    // Извлекает серийные номера модулей памяти из SMBIOS и возвращает их SHA-256-хеш
    
    internal class RamSerialHash : IHardwareComponent
    {
        private const uint RSMB = 0x52534D42;


        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint GetSystemFirmwareTable(
            uint firmwareTableProviderSignature,
            uint firmwareTableID,
            IntPtr pFirmwareTableBuffer,
            uint bufferSize);



        public string Key => "RAM";
        public string GetValue() => ComputeSerialHash();

 



        private static string ComputeSerialHash()
        {
            string raw = GetRawSerials();
            if (string.IsNullOrEmpty(raw))
                return "UNKNOWN_HASH";

            SHA256 sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }

        private static string GetRawSerials()
        {
            uint size = GetSystemFirmwareTable(RSMB, 0, IntPtr.Zero, 0);
            if (size == 0)
                return string.Empty;

            IntPtr ptr = Marshal.AllocHGlobal((int)size);
            try
            {
                uint written = GetSystemFirmwareTable(RSMB, 0, ptr, size);
                if (written != size)
                    return string.Empty;

                byte[] raw = new byte[size];
                Marshal.Copy(ptr, raw, 0, (int)size);

                return ParseSmBios(raw);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        // ---------- разбор SMBIOS ----------

        private static string ParseSmBios(byte[] data)
        {
            var serials = new List<string>();

            if (data.Length < 8)
                return string.Empty;

            int tableLen = BitConverter.ToInt32(data, 4);
            int pos = 8;
            int tableEnd = Math.Min(pos + tableLen, data.Length);

            while (pos + 4 <= tableEnd)
            {
                byte type = data[pos];
                byte length = data[pos + 1];

                if (length == 0 || pos + length > tableEnd)
                    break;

                // Type 17 — Memory Device; смещение 0x18 = индекс строки Serial Number
                if (type == 17 && length > 0x18)
                {
                    byte idx = data[pos + 0x18];
                    if (idx != 0)
                    {
                        string sn = GetSmbiosString(data, pos + length, tableEnd, idx);
                        if (!string.IsNullOrWhiteSpace(sn))
                            serials.Add(sn.Trim());
                    }
                }

                // переход к следующей структуре
                int next = pos + length;
                while (next + 1 < tableEnd &&
                       (data[next] != 0 || data[next + 1] != 0))
                    next++;

                next += 2;  // пропуск двойного терминатора
                pos = next;
            }

            return string.Join("|",
                serials
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(s => s, StringComparer.OrdinalIgnoreCase));
        }

        private static string GetSmbiosString(byte[] data, int start, int end, int index)
        {
            int pos = start, cur = 1;

            while (pos < end && data[pos] != 0)
            {
                int strEnd = pos;
                while (strEnd < end && data[strEnd] != 0)
                    strEnd++;

                if (cur == index)
                    return Encoding.ASCII.GetString(data, pos, strEnd - pos);

                pos = strEnd + 1;
                cur++;
            }

            return string.Empty;
        }
    }
}
