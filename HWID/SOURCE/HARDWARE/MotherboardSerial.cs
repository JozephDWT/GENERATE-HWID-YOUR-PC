using HWID.SOURCE.HELPER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace HWID.SOURCE.HARDWARE
{
    public sealed class MotherboardSerial : IHardwareComponent
    {
        public string Key => "Мат.плата";

        public string GetValue()
        {
            const string query = "SELECT SerialNumber FROM Win32_BaseBoard";
            try
            {
                var mos = new ManagementObjectSearcher(query);
                foreach (ManagementObject mo in mos.Get())
                    return mo["SerialNumber"]?.ToString()?.Trim() ?? string.Empty;
            }
            catch { }
            return string.Empty;
        }
    }
}
