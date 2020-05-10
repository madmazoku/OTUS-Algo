using System;
using System.Numerics;

namespace lesson._02.cs
{
    class FibonacciRecursiveTask : FibonacciTask
    {
        public override string Name() { return "Рекурсивный"; }

        public override BigInteger Fibonacci(BigInteger n)
        {
            return FibonacciStackLimit(n, 0);
        }

        static long maxStackSize = 3000;

        public BigInteger FibonacciStackLimit(BigInteger n, long level)
        {
            if (level > maxStackSize)
                throw new StackOverflowException();

            if (n < 2)
                return n;
            else
                return FibonacciStackLimit(n - 2, level + 1) + FibonacciStackLimit(n - 1, level + 1);
        }
    }
}
