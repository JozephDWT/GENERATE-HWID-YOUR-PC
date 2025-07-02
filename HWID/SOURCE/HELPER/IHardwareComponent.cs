using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWID.SOURCE.HELPER
{
    public interface IHardwareComponent
    {
        string Key { get; }
        string GetValue();
    }
}
