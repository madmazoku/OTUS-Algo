using System;

namespace lesson._27.cs
{
    class FiveEight
    {
        static public void Do()
        {
            long N = long.Parse(Console.ReadLine().Trim());
            long K;
            if (N <= 0)
                K = 0;
            else if (N == 1)
                K = 2;
            else
            {
                long[] x = { 1, 1, 1, 1 };
                long[] n = { 0, 0, 0, 0 };
                while (N > 1)
                {
                    n[0] = x[1] + x[3];
                    n[1] = x[0] + x[2];
                    n[2] = x[0];
                    n[3] = x[1];
                    Array.Copy(n, x, 4);
                    --N;
                }
                K = x[0] + x[1] + x[2] + x[3];
            }
            Console.WriteLine(K);
        }
    }
}
