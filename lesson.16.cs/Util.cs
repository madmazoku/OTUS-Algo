using System;

namespace lesson._16.cs
{
    class Util
    {
        static public void Print((int, int)[] array)
        {
            if (array.Length == 0)
                Console.WriteLine("Empty");
            else
            {
                (int from, int to) = array[0];
                Console.Write($"{from,2} -> {to,2}");
                for (int y = 1; y < array.Length; ++y)
                {
                    (from, to) = array[y];
                    Console.Write($"; {from,2} -> {to,2}");
                }
                Console.WriteLine("");
            }
        }

        static public void Print(int[] array)
        {
            if (array.Length == 0)
                Console.WriteLine("Empty");
            else
            {
                Console.Write($"{array[0],2}");
                for (int y = 1; y < array.Length; ++y)
                    Console.Write($"; {array[y],2}");
                Console.WriteLine("");
            }
        }

        static public void Print(int[][] skewArray)
        {
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) = (Console.ForegroundColor, Console.BackgroundColor);
            for (int y = 0; y < skewArray.Length; ++y)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($" {y,2}:");
                Console.ForegroundColor = ConsoleColor.White;
                int[] subArray = skewArray[y];
                if (subArray.Length > 0)
                    for (int x = 0; x < subArray.Length; ++x)
                    {
                        Console.BackgroundColor = (y & 0x1) == 0 ? ConsoleColor.Black : ConsoleColor.DarkGray;
                        Console.Write($" {subArray[x],2} ");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write("|");
                    }
                else
                {
                    Console.BackgroundColor = (y & 0x1) == 0 ? ConsoleColor.Black : ConsoleColor.DarkGray;
                    Console.Write($"    ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("|");
                }
                Console.WriteLine("");
            }
            (Console.ForegroundColor, Console.BackgroundColor) = (foregroundColor, backgroundColor);
            Console.WriteLine("");
        }

        static public void Print(bool[,] matrix)
        {
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) = (Console.ForegroundColor, Console.BackgroundColor);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"    ");
            for (int x = 0; x < matrix.GetLength(1); ++x)
                Console.Write($"{x,3}|");
            Console.Write($"    ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            for (int y = 0; y < matrix.GetLength(0); ++y)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"{y,-3}|");
                Console.ForegroundColor = ConsoleColor.White;
                for (int x = 0; x < matrix.GetLength(1); ++x)
                {
                    Console.BackgroundColor = (y & 0x1) == 0 ? ConsoleColor.Black : ConsoleColor.DarkGray;
                    Console.Write(matrix[y, x] ? " 1 " : " 0 ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("|");
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"{y,3}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"    ");
            for (int x = 0; x < matrix.GetLength(1); ++x)
                Console.Write($"{x,3}|");
            Console.Write($"    ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            (Console.ForegroundColor, Console.BackgroundColor) = (foregroundColor, backgroundColor);
            Console.WriteLine("");
        }

        static public T[][] SkewListToArray<T>(NodeStack<NodeStack<T>> skewStack)
        {
            T[][] skewArray = new T[skewStack.size][];
            while (skewStack.size > 0)
            {
                NodeStack<T> stack = skewStack.Pop();
                skewArray[skewStack.size] = Util.ListToArray<T>(stack);
            }
            return skewArray;
        }

        static public T[][] SkewListToArray<T>(NodeStack<NodeQueue<T>> skewStackQueue)
        {
            T[][] skewArray = new T[skewStackQueue.size][];
            while (skewStackQueue.size > 0)
            {
                NodeQueue<T> queue = skewStackQueue.Pop();
                skewArray[skewStackQueue.size] = Util.ListToArray<T>(queue);
            }
            return skewArray;
        }

        static public T[] ListToArray<T>(NodeStack<T> stack)
        {
            T[] array = new T[stack.size];
            while (stack.size > 0)
            {
                T value = stack.Pop();
                array[stack.size] = value;
            }
            return array;
        }

        static public T[] ListToArray<T>(NodeQueue<T> queue)
        {
            T[] array = new T[queue.size];
            while (queue.size > 0)
            {
                T value = queue.Deque();
                array[queue.size] = value;
            }
            return array;
        }

    }
}
