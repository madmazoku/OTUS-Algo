using System;
using System.Collections.Generic;
using System.Text;

namespace lesson._03.cs
{
    class Utils
    {
        public static int CountBits(ulong x)
        {
            int s = 0;
            while (x > 0)
            {
                x &= x - 1;
                ++s;
            }
            return s;
        }
    }
}
