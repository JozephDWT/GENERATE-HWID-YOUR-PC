using System;
using System.Collections.Generic;
using HWID.SOURCE.HELPER;
using HWID.SOURCE.HARDWARE;
using HWID.SOURCE.HELPER.Hasher;

namespace HWID.SOURCE
{
    internal class Program
    {
        // 3 из 4 компонентов совпало → считаем, что это тот же ПК
        private const int Match = 3;

        static void Main()
        {
            var providers = new List<IHardwareComponent>
            {
                new MachineGuid(),
                new CpuId(),
                new MotherboardSerial(),
                new SytemDiskSerial()
            };

            var collector = new HardwareCollector(providers);
            var hasher = new HwidHasher();
            var cacheManager = new CacheManager();
            var report = new Writer(hasher);

            Dictionary<string, string> Parts = collector.Collect();
            string Hwid = hasher.ComputeOverall(Parts);

            string finalHwid = cacheManager.ResolveHwid(
                                    Hwid,
                                    Parts,
                                    Match);

            report.WriteDesktopReport(finalHwid, Parts);

            Console.WriteLine("готово! Нажми 'Enter' для выхода.");
            Console.ReadLine();
        }
    }
}
