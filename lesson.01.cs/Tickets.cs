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
            return Happy(int.Parse(data[0])).ToString();
        }

        long Happy(int size)
        {
            long[][] cs = new long[size + 1][];
            cs[0] = new long[1];
            cs[0][0] = 1;

            for (int i = 1; i <= size; i++)
            {
                cs[i] = new long[i * 9 + 1];
                for(int k = 0; k <= 9*(i-1); k++)
                {
                    long n = cs[i - 1][k];
                    for (int c = 0; c <= 9; c++)
                    {
                        cs[i][k + c] += n;
                    }
                }
            }

            long fc = 0;
            for (int i = 0; i <= size * 9; i++)
            {
                fc += cs[size][i] * cs[size][i];
            }

            return fc;
        }
/*
        long Happy(int size)
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
            return c;
        }
*/
/*
        int Happy(int size)
        {
            int[] a = new int[size];
            int[] b = new int[size - 1];
            int c = 0;

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
            }
            return c;
        }
*/
/*
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
*/
    }
}
