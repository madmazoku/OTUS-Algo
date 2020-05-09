using System.Numerics;

namespace lesson._02.cs
{
    abstract class GCDTask : ITask
    {
        private BigInteger a;
        private BigInteger b;
        private BigInteger gcd;

        public abstract string Name();

        public void Prepare(string[] data)
        {
            a = BigInteger.Parse(data[0]);
            b = BigInteger.Parse(data[1]);
            gcd = 0;
        }

        public void Run()
        {
            gcd = GCD(a, b);
        }

        public bool Result(string expected)
        {
            BigInteger expectedGCD = BigInteger.Parse(expected);
            return gcd == expectedGCD;
        }

        public abstract BigInteger GCD(BigInteger a, BigInteger b);
    }
}
