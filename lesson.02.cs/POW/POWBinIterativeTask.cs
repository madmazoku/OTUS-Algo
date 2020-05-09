using System;

namespace lesson._02.cs
{
    class POWBinIterativeTask : POWTask
    {
        public override string Name() { return "Итеративный 2^n"; }

        public override double POW(double x, long y)
        {
            double r = 1;
            while(y > 0)
            {
                if ((y & 0x1) == 1)
                    r *= x;
                y >>= 1;
                x *= x;
            }
            return r;
        }
    }
}
