using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWID.SOURCE.HARDWARE
{
    internal class GetString
    {
        public static string Get(byte[] data, int start, int end, int index)
        {
            int a = start;
            int b = 1;

            while (a < end && data[a] != 0)
            {
                int strEnd = a;
                while (strEnd < end && data[strEnd] != 0)
                    strEnd++;

                if (b == index)
                    return Encoding.ASCII.GetString(data, a, strEnd - b);

                a = strEnd + 1;
                b++;
            }

            return string.Empty;
        }
    }
}
