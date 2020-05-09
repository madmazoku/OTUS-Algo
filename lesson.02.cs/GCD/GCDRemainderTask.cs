using System.Numerics;

namespace lesson._02.cs
{
    class GCDRemainderTask : GCDTask
    {
        public override string Name() { return "Через остаток"; }

        public override BigInteger GCD(BigInteger a, BigInteger b)
        {
            while (a != 0 && b != 0)
                if (a > b)
                    a %= b;
                else
                    b %= a;
            return a != 0 ? a : b;
        }
    }
}
