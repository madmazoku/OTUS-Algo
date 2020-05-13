using System.Numerics;

namespace lesson._02.cs
{
    class FibonacciIterativeTask : FibonacciTask
    {
        public override string Name() { return "Итеративный"; }

        public override BigInteger Fibonacci(BigInteger n)
        {
            if (n < 2)
                return n;

            BigInteger p = 0;
            BigInteger f = 1;
            while (n >= 2)
            {
                BigInteger t = f;
                f += p;
                p = t;
                --n;
            }

            return f;
        }
    }
}
