using System;
using System.Collections.Generic;
using HWID.SOURCE.HELPER;
using HWID.SOURCE.HARDWARE;
using HWID.SOURCE.HELPER.Hasher;

namespace HWID.SOURCE
{
    internal class Program
    {
        static void Main()
        {
            // Список идентификаторов
            var providers = new List<IHardwareComponent>
            {
                new CpuId(), //проц-CPU
                new MotherboardSerial(), //мат-плата-Motheboard
                new RamSerialHash() // озу-RAM
            };

            var collector = new HardwareCollector(providers);
            var hasher = new HwidHasher();
            var cacheManager = new CacheManager();
            var reportWriter = new Writer(hasher);

            // Сбор данных
            Dictionary<string, string> parts = collector.Collect();

            string hwid = hasher.ComputeOverall(parts);
            string finalHwid = cacheManager.ResolveHwid(hwid, parts);

            // Отчёт
            reportWriter.WriteDesktopReport(finalHwid, parts);

            Console.WriteLine("Готово! Нажмите Enter для выхода.");
            Console.ReadLine();
        }
    }
}
