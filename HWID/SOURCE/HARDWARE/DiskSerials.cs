using HWID.SOURCE.HELPER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace HWID.SOURCE.HARDWARE
{
    public sealed class DiskSerials : IHardwareComponent
    {
        public string Key => "Диски";

        public string GetValue()
        {
            var serials = Logic();
            return serials.Count > 0 ? string.Join(", ", serials) : "UNKNOWN_SERIALS";
        }

        private static List<string> Logic()
        {
            var serials = new List<string>();

            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT SerialNumber, MediaType FROM Win32_DiskDrive"))
                {
                    serials.AddRange(from ManagementObject disk in searcher.Get()
                                     let mediaType = disk["MediaType"]?.ToString()
                                     where mediaType != null && mediaType.ToLower().Contains("fixed")
                                     let raw = disk["SerialNumber"]?.ToString()?.Trim()
                                     where !string.IsNullOrEmpty(raw)
                                     select raw);
                }
            }
            catch
            {}

            return serials;
        }
    }
}
