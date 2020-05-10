using System;
using System.Numerics;

namespace lesson._02.cs
{
    abstract class FibonacciTask : ITask
    {
        private BigInteger n;
        private BigInteger fibonacci;

        public abstract string Name();

        public void Prepare(string[] data)
        {
            n = long.Parse(data[0]);
            fibonacci = 0;
        }

        public void Run()
        {
            fibonacci = Fibonacci(n);
        }

        public bool Result(string expected)
        {
            BigInteger expectedFibonacci = BigInteger.Parse(expected);
            return expectedFibonacci == fibonacci;
        }

        public abstract BigInteger Fibonacci(BigInteger n);
    }
}
