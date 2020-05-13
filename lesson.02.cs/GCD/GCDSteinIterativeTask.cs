using System.Numerics;

namespace lesson._02.cs
{
    class GCDSteinIterativeTask : GCDTask
    {
        public override string Name() { return "Итеративный Стейна"; }

        public override BigInteger GCD(BigInteger a, BigInteger b)
        {
            int s = 0;
            if (a == b || b == 0) return a;
            if (a == 0) return b;

            while (((a | b) & 0x1) == 0)
            {
                ++s;
                a >>= 1;
                b >>= 1;
            }

            while ((a & 0x1) == 0)
                a >>= 1;

            do
            {
                while ((b & 0x1) == 0)
                    b >>= 1;

                if (a > b)
                {
                    BigInteger t = a;
                    a = b;
                    b = t;
                }

                b -= a;
            } while (b != 0);

            return a << s;
        }
    }
}
