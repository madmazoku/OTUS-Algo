using System;
using System.Collections.Generic;
using System.Text;

namespace lesson._02.cs
{
    abstract class PrimesTask : ITask
    {
        private long n;
        private long primes;

        public abstract string Name();

        public void Prepare(string[] data)
        {
            n = long.Parse(data[0]);
            primes = 0;
        }

        public void Run()
        {
            primes = Primes(n);
        }

        public bool Result(string expected)
        {
            long expectedPrimes = long.Parse(expected);
            return expectedPrimes == primes;
        }

        public abstract long Primes(long n);
    }
}
