using HWID.SOURCE.HELPER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace HWID.SOURCE.HARDWARE
{
    public sealed class CpuId : IHardwareComponent
    {
        public string Key => "Процессор";

        public string GetValue()
        {
            const string query = "SELECT ProcessorId FROM Win32_Processor";
            try
            {
                var mos = new ManagementObjectSearcher(query);
                foreach (ManagementObject mo in mos.Get())
                    return mo["ProcessorId"]?.ToString()?.Trim() ?? string.Empty;
            }
            catch { }
            return string.Empty;
        }
    }
}
