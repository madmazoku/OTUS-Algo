using System;
using System.Collections.Generic;
using System.Linq;

namespace lesson._27.cs
{
    class Program
    {

        static public int GCD(int a, int b)
        {
            if (a == b || b == 0)
                return a;
            if (a == 0)
                return b;

            if ((a & 0x1) == 0)
            {
                if ((b & 0x1) == 0)
                    return GCD(a >> 1, b >> 1) << 1;
                else
                    return GCD(a >> 1, b);
            }
            else
            {
                if ((b & 0x1) == 0)
                    return GCD(a, b >> 1);
                else
                {
                    if (a > b)
                        return GCD((a - b) >> 1, b);
                    else
                        return GCD(a, (b - a) >> 1);
                }
            }
        }

        static public void Peas()
        {
            int[] args = Console.ReadLine().Trim().Split(new char[] { '+', '/' }).Select(x => int.Parse(x)).ToArray();
            int n = args[0] * args[3] + args[2] * args[1];
            int d = args[1] * args[3];
            int g = GCD(n, d);
            n /= g;
            d /= g;
            Console.WriteLine($"{n}/{d}");
        }

        static public void NumberFir()
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

        static public void FiveEight()
        {
            long N = long.Parse(Console.ReadLine().Trim());
            long K;
            if (N <= 0)
                K = 0;
            else if (N == 1)
                K = 2;
            else if (N == 2)
                K = 4;
            else
            {
                long[] x = { 1, 1, 1, 1 };
                long[] n = { 0, 0, 0, 0 };
                while (N > 2)
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

        static public void Islands()
        {
            int N = int.Parse(Console.ReadLine());
            int[][] matrix = new int[N][];
            for (int row = 0; row < N; ++row)
                matrix[row] = Console.ReadLine().Trim().Split(' ').Select(x => int.Parse(x)).ToArray();

            Stack<int> stack = new Stack<int>();
            int K = 0;
            for (int row = 0; row < N; ++row)
                for (int col = 0; col < N; ++col)
                    if (matrix[row][col] == 1)
                    {
                        ++K;
                        stack.Push(col); stack.Push(row);
                        while(stack.Count > 0)
                        {
                            int y = stack.Pop();
                            int x = stack.Pop();
                            if (x >= 0 && x < N && y >= 0 && y < N && matrix[y][x] == 1)
                            {
                                matrix[y][x] = 0;
                                stack.Push(x); stack.Push(y + 1);
                                stack.Push(x + 1); stack.Push(y);
                                stack.Push(x); stack.Push(y - 1);
                                stack.Push(x - 1); stack.Push(y);
                            }
                        }
                    }

            Console.WriteLine(K);
        }

        //struct Warehouse
        //{
        //    public int square;
        //    public int col;
        //    public int row;
        //    public int width;
        //    public int height;

        //    public Warehouse(int square, int col, int row, int width, int height)
        //    {
        //        this.square = square;
        //        this.col = col;
        //        this.row = row;
        //        this.width = width;
        //        this.height = height;
        //    }

        //    public override string ToString()
        //    {
        //        return $"col: {col}; row: {row}; width: {width}; height: {height}; square: {square}";
        //    }
        //};

        //static void SmallWarehouse()
        //{
        //    int[] args = Console.ReadLine().Trim().Split(' ').Select(x => int.Parse(x)).ToArray();
        //    int N = args[0];
        //    int M = args[1];

        //    int[] line = new int[N];
        //    Warehouse warehouse = new Warehouse(0, -1, -1, 0, 0);
        //    for (int row = 0; row < M; ++row)
        //    {
        //        int[] input = Console.ReadLine().Trim().Split(' ').Select(x => int.Parse(x)).ToArray();

        //        for (int col = 0; col < N; ++col)
        //            if (input[col] == 1)
        //                line[col] = 0;
        //            else
        //                ++line[col];

        //        for(int col = 0; col < N; ++col)
        //        {
        //            int hCurrent = line[col];
        //            for(int col2 = col, w = 1; col2 < N; ++col2, ++w)
        //            {
        //                if (input[col] == 1)
        //                    break;
        //                if (hCurrent > line[col2])
        //                    hCurrent = line[col2];
        //                int s = w * hCurrent;
        //                if (warehouse.square < s)
        //                    warehouse = new Warehouse(s, col, row - hCurrent + 1, w, hCurrent);
        //            }
        //        }
        //    }

        //    Console.WriteLine(warehouse.ToString());
        //}

        static void SmallWarehouse()
        {
            int[] args = Console.ReadLine().Trim().Split(' ').Select(x => int.Parse(x)).ToArray();
            int N = args[0];
            int M = args[1];

            int[] line = new int[N];
            int[] left = new int[N];
            int[] right = new int[N];
            Stack<int> stack = new Stack<int>();
            int sMax = 0;
            for (int row = 0; row < M; ++row)
            {
                int[] input = Console.ReadLine().Trim().Split(' ').Select(x => int.Parse(x)).ToArray();

                for (int col = 0; col < N; ++col)
                    if (input[col] == 1)
                        line[col] = 0;
                    else
                        ++line[col];

                {
                    for (int col = 0; col < N; ++col)
                    {
                        while (stack.Count > 0 && line[col] < line[stack.Peek()])
                            right[stack.Pop()] = col - 1;
                        if (line[col] > 0)
                            stack.Push(col);
                    }
                    while (stack.Count > 0)
                        right[stack.Pop()] = N - 1;
                }

                {
                    for (int col = N - 1; col >= 0; --col)
                    {
                        while (stack.Count > 0 && line[col] < line[stack.Peek()])
                            left[stack.Pop()] = col + 1;
                        if (line[col] > 0)
                            stack.Push(col);
                    }
                    while (stack.Count > 0)
                        left[stack.Pop()] = 0;
                }

                for (int col = 0; col < N; ++col)
                    if (line[col] > 0)
                    {
                        int s = (right[col] - left[col] + 1) * line[col];
                        if (sMax < s)
                            sMax = s;
                        // square: {s}; col: {left[col]}; row: {row - line[col] + 1}; width: {right[col] - left[col] + 1}; height: {line[col]}
                    }
            }

            Console.WriteLine(sMax);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Dynamic Programming");
            //Peas();
            //NumberFir();
            //FiveEight();
            //Islands();
            //SmallWarehouse();
        }
    }
}
