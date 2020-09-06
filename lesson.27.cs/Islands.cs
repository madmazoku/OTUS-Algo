using System;
using System.Collections.Generic;
using System.Linq;

namespace lesson._27.cs
{
    class Islands
    {
        static public void Do()
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
                        while (stack.Count > 0)
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
    }
}
