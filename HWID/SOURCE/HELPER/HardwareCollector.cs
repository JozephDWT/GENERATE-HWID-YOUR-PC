using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWID.SOURCE.HELPER
{
    public class HardwareCollector
    {
        private readonly IEnumerable<IHardwareComponent> _providers;

        public HardwareCollector(
            IEnumerable<IHardwareComponent> providers)
        {
            _providers = providers;
        }

        public Dictionary<string, string> Collect() =>
            _providers.ToDictionary(p => p.Key, p => p.GetValue());
    }
}
