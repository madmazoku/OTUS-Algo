using System;
using System.Numerics;

namespace lesson._02.cs
{
    class FibonacciGoldenTask : FibonacciTask
    {
        public override string Name() { return "Золотое сечение"; }

        static decimal sqrt5 = 2.2360679774997896964091736688M;
        static decimal phi = 1.6180339887498948482045868344M;

        decimal pow(decimal x, BigInteger y)
        {
            decimal r = 1;
            while (y > 0)
            {
                if ((y & 0x1) == 1)
                    r *= x;
                y >>= 1;
                x *= x;
            }
            return r;
        }

        public override BigInteger Fibonacci(BigInteger n)
        {
            if (n < 2)
                return n;

            BigInteger f = (BigInteger)Math.Floor(pow(phi, n) / sqrt5 + 0.5M);

            return f;
        }
    }
}
