using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace lesson._02.cs
{
    class PrimesEratostheneFastTask : PrimesTask
    {
        public override string Name() { return "Эратосфен, O(n)"; }

        public override long Primes(long n)
        {
            long[] pr = new long[n];
            long pri = 0;
            long[] lp = new long[n + 1];

            for (long i = 2; i <= n; ++i)
            {
                if (lp[i] == 0)
                {
                    lp[i] = i;
                    pr[pri++] = i;
                }
                for (long j = 0; j < pri; ++j)
                {
                    long p = pr[j];
                    if (p <= lp[i] && p * i <= n)
                        lp[p * i] = p;
                    else
                        break;
                }
            }

            return pri;
        }
    }
}

