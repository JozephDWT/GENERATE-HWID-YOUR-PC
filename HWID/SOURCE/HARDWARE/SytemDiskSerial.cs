using HWID.SOURCE.HELPER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace HWID.SOURCE.HARDWARE
{
    public sealed class SytemDiskSerial : IHardwareComponent
    {
        public string Key => "Диск (на котором установлена OC)";

        // извлекает серийный номер диска (C) 
        public string GetValue()
        {
            const string query =
                "SELECT VolumeSerialNumber FROM Win32_LogicalDisk WHERE DeviceID='C:'";
            try
            {
                var mos = new ManagementObjectSearcher(query);
                foreach (ManagementObject mo in mos.Get())
                    return mo["VolumeSerialNumber"]?.ToString()?.Trim() ?? string.Empty;
            }
            catch { }
            return string.Empty;
        }
    }
}
