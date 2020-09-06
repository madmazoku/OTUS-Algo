using System;
using System.Collections.Generic;
using System.Linq;

namespace lesson._27.cs
{
    class SmallWarehouse
    {
        static public void Do()
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
    }
}
