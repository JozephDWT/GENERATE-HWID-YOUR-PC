using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace HWID.SOURCE.HARDWARE
{
    // Достаёт «сырые» таблицы SMBIOS из прошивки и отдаёт коллекцию серийных номеров RAM.

    internal static class RamSerialReader
    {
        private const uint RSMB = 0x52534D42;   // 'RSMB'

        //P/Invoke
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint GetSystemFirmwareTable(
            uint providerSignature,
            uint tableId,
            IntPtr buffer,
            uint bufferSize);
        

        public static IReadOnlyCollection<string> ReadAll()
        {
            var raw = ReadRawBuffer();
            return raw?.Length > 0
                ? SmbiosParser.ExtractRamSerials(raw)
                : Array.Empty<string>();
        }

        private static byte[] ReadRawBuffer()
        {
            uint size = GetSystemFirmwareTable(RSMB, 0, IntPtr.Zero, 0);
            if (size == 0) return null;

            IntPtr ptr = Marshal.AllocHGlobal((int)size);
            try
            {
                uint written = GetSystemFirmwareTable(RSMB, 0, ptr, size);
                if (written != size) return null;

                var buffer = new byte[size];
                Marshal.Copy(ptr, buffer, 0, (int)size);
                return buffer;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
