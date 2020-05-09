using System;
using System.Collections.Generic;
using System.Text;

namespace lesson._02.cs
{
    class PrimesListTask : PrimesTask
    {
        public override string Name() { return "Список"; }

        bool isPrime(long n, List<long> primes)
        {
            if ((n & 0x1) == 0)
                return n == 2;

            long s = (long)Math.Sqrt(n);
            foreach(long i in primes)
                if (i > s)
                    break;
                else if (n % i == 0)
                    return false;

            return true;
        }

        public override long Primes(long n)
        {
            List<long> primes = new List<long>();
            for (long i = 2; i <= n; ++i)
                if (isPrime(i, primes))
                    primes.Add(i);
            return primes.Count;
        }
    }
}
