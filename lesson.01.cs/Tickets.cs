using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;
using System.Timers;

namespace lesson._01.cs
{
    class Tickets : ITask
    {
        public string Run(string[] data)
        {
            return Happy2(int.Parse(data[0])).ToString();
        }

        long lPow(long x, long y)
        {
            long r = 1;
            for (int i = 0; i < y; i++)
                r *= x;

            //while (y != 0)
            //{
            //    if ((y & 1) == 1)
            //        r *= x;
            //    y >>= 1;
            //    r *= r;
            //}

            return r;
        }

        long Happy2(int size)
        {
            long[] cs = new long[size * 9 + 1];
            int[] a = new int[size];
            while (true)
            {
                int sum = Next(a, size);
                cs[sum]++;
                if (sum == 0) break;
            }
            long c = 0;
            for(int i = 0; i <= size * 9; i++)
            {
                c += cs[i] * cs[i];
            }
            Console.WriteLine($"size: {size}; happy: {c}");
            return c;
        }

        int Happy1(int size)
        {
            int[] a = new int[size];
            int[] b = new int[size - 1];
            int c = 0;
            Stopwatch t = new Stopwatch();
            t.Start();
            long cycles = 0;
            long start = t.ElapsedMilliseconds;
            long prev = start;
            long cur = start;
            long full = lPow(10, size*2-1);
            Console.WriteLine($"size: {size}; full cycles: {full}");
            while (true)
            {
                int sum_a = Next(a, size);
                while (true)
                {
                    int sum_b = Next(b, size - 1);
                    if (sum_a >= sum_b && sum_a <= sum_b + 9)
                        c++;
                    cycles++;
                    if (sum_b == 0) break;
                }
                if (sum_a == 0) break;
                cur = t.ElapsedMilliseconds;
                if(cur - prev >= 100)
                {
                    Console.WriteLine($"{cur - start}: cycles: {cycles}; happy: {c}; left: {1.0 * (full - cycles) / full * 100}% / {1.0 * (cur-start) / cycles * (full - cycles)}");
                    prev = cur;
                }
            }
            t.Stop();
            cur = t.ElapsedMilliseconds;
            Console.WriteLine($"size: {size}; ms: {cur - start}; cycles: {cycles}; happy: {c}");
            return c;
        }

        int Next(int[] a, int size)
        {
            int sum = 0;
            int i = 0;
            while(i < size)
            {
                if(a[i] < 9)
                {
                    a[i]++;
                    break;
                }
                else
                {
                    a[i] = 0;
                    i++;
                }
            }
            while(i < size)
            {
                sum += a[i];
                i++;
            }
            return sum;
        }
    }
}
