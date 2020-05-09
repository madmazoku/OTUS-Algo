using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace lesson._02.cs
{
    class PrimesEratostheneBitsTask : PrimesTask
    {
        public override string Name() { return "Эратосфен, битовый"; }

        public override long Primes(long n)
        {
            BitArray not_primes = new BitArray((int)n);
            long s = (long)Math.Sqrt(n);
            long primes = 0;
            for(long i = 2; i <= s; ++i)
            {
                if (not_primes.Get((int)(i - 1))) continue;
                ++primes;
                for (long j = i * i; j <= n; j += i)
                    not_primes.Set((int)(j - 1), true);
            }
            for (long i = s + 1; i <= n; ++i)
                if (!not_primes.Get((int)(i - 1)))
                    ++primes;
            return primes;
        }
    }
}
