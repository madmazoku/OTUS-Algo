using System;

namespace lesson._16.cs
{
    class Util
    {
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

        static public T[][] SkewListToArray<T>(int countLevels, Node<Node<T>> rootLevel, Node<int> rootLevelSize)
        {
            T[][] skewArray = new T[countLevels][];
            while (countLevels > 0)
            {
                --countLevels;

                skewArray[countLevels] = new T[rootLevelSize.value];
                while (rootLevelSize.value > 0)
                {
                    --rootLevelSize.value;
                    skewArray[countLevels][rootLevelSize.value] = rootLevel.value.value;
                    rootLevel.value = rootLevel.value.next;
                }

                rootLevel = rootLevel.next;
                rootLevelSize = rootLevelSize.next;
            }

            return skewArray;
        }

        static public T[] ListToArray<T>(int count, Node<T> root)
        {
            T[] array = new T[count];
            while (count > 0)
            {
                array[--count] = root.value;
                root = root.next;
            }
            return array;
        }

        static public void MoveNodeBetweenLists<T>(ref Node<T> from, ref Node<T> to)
        {
            Node<T> tmp = from;
            from = from.next;
            tmp.next = to;
            to = tmp;
        }

        static public void ReverseList<T>(ref Node<T> node)
        {
            Node<T> reverseNode = null;
            while (node != null)
                MoveNodeBetweenLists<T>(ref node, ref reverseNode);
            node = reverseNode;
        }
    }
}
