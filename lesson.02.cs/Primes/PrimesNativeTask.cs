using System;

namespace lesson._02.cs
{
    class PrimesNativeTask : PrimesTask
    {
        public override string Name() { return "Нативный"; }

        bool isPrime(long n)
        {
            if ((n & 0x1) == 0)
                return n == 2;

            long s = (long)Math.Sqrt(n);
            for (long i = 3; i <= s; i += 2)
                if (n % i == 0)
                    return false;

            return true;
        }

        public override long Primes(long n)
        {
            long cnt = 0;
            for (long i = 2; i <= n; ++i)
                if (isPrime(i))
                    ++cnt;
            return cnt;
        }
    }
}
