using System;

namespace lesson._02.cs
{
    class POWBinRecursiveTask : POWTask
    {
        public override string Name() { return "Рекурсивный 2^n"; }

        public override double POW(double x, long y)
        {
            if (y == 0)
                return 1;
            else
            {
                double r = POW(x, y >> 1);
                r *= r;
                if ((y & 0x1) == 1)
                    r *= x;
                return r;
            }
        }
    }
}
