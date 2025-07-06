using HWID.SOURCE.HARDWARE;
using HWID.SOURCE.HELPER;
using HWID.SOURCE.HELPER.Hasher;
using System.Linq;

namespace HWID.SOURCE.HARDWARE
{
    // отдаёт SHA-256 от всех серийников модулей ОЗУ

    internal class RamHashMain : IHardwareComponent
    {
        public string Key => "RAM";

        public string GetValue()
        {
            var a = RamSerialReader.ReadAll();
            var b = string.Join("|", a);

            return SerialHasher.ComputeSha256(b);
        }
    }
}
