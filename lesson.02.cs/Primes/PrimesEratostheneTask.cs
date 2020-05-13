using System;

namespace lesson._02.cs
{
    class PrimesEratostheneTask : PrimesTask
    {
        public override string Name() { return "Эратосфен"; }

        public override long Primes(long n)
        {
            bool[] not_primes = new bool[n];
            long s = (long)Math.Sqrt(n);
            long primes = 0;
            for (long i = 2; i <= s; ++i)
            {
                if (not_primes[i - 1]) continue;
                ++primes;
                for (long j = i * i; j <= n; j += i)
                    not_primes[j - 1] = true;
            }
            for (long i = s + 1; i <= n; ++i)
                if (!not_primes[i - 1])
                    ++primes;
            return primes;
        }
    }
}
