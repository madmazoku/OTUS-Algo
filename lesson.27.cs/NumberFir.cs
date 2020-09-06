using System;
using System.Linq;

namespace lesson._27.cs
{
    class NumberFir
    {
        static public void Do()
        {
            int N = int.Parse(Console.ReadLine().Trim());
            int[][] fir = new int[N][];
            for (int row = 0; row < N; ++row)
                fir[row] = Console.ReadLine().Trim().Split(' ').Select(x => int.Parse(x)).ToArray();
            for (int row = N - 2; row >= 0; --row)
                for (int col = 0; col < fir[row].Length; ++col)
                    fir[row][col] += Math.Max(fir[row + 1][col], fir[row + 1][col + 1]);
            Console.WriteLine(fir[0][0]);
        }
    }
}
