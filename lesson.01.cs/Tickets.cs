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

    }
}
