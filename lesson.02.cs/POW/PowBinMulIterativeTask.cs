using System;

namespace lesson._02.cs
{
    class POWBinMulIterateTask : POWTask
    {
        public override string Name() { return "2^n * Итеративный"; }

        public override double POW(double x, long y)
        {
            if (y == 0)
                return 1;
            long n = 0;
            double r = x;
            while ((y & 0x1) == 0)
            {
                ++n;
                y >>= 1;
            }
            while(y > 1)
            {
                r *= x;
                --y;
            }
            while(n > 0)
            {
                r *= r;
                --n;
            }
            return r;
        }
    }
}
