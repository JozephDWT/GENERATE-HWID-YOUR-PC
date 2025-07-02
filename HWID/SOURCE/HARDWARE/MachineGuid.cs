using HWID.SOURCE.HELPER;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWID.SOURCE.HARDWARE
{
    public sealed class MachineGuid : IHardwareComponent
    {
        public string Key => "MachineGuid (из реестра)";

        public string GetValue()
        {
            try
            {
                var rk = Registry.LocalMachine.OpenSubKey(
                               @"SOFTWARE\Microsoft\Cryptography");
                return rk?.GetValue("MachineGuid")?.ToString() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
