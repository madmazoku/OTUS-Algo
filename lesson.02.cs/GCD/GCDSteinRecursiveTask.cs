using System.Numerics;

namespace lesson._02.cs
{
    class GCDSteinRecursiveTask : GCDTask
    {
        public override string Name() { return "Рекурсивный Стейна"; }

        public override BigInteger GCD(BigInteger a, BigInteger b)
        {
            if (a == b || b == 0)
                return a;
            if (a == 0)
                return b;

            if ((a & 0x1) == 0)
                if ((b & 0x1) == 0)
                    return GCD(a >> 1, b >> 1) << 1;
                else
                    return GCD(a >> 1, b);
            else
                if ((b & 0x1) == 0)
                return GCD(a, b >> 1);
            else
                    if (a > b)
                return GCD((a - b) >> 1, b);
            else
                return GCD(a, (b - a) >> 1);
        }

    }
}
