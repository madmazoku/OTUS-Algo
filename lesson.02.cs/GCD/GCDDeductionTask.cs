using System.Numerics;

namespace lesson._02.cs
{
    class GCDDeductionTask : GCDTask
    {
        public override string Name() { return "Через вычитание"; }

        public override BigInteger GCD(BigInteger a, BigInteger b)
        {
            while (a != b)
                if (a > b)
                    a = a - b;
                else
                    b = b - a;
            return a;
        }
    }
}
