using System;

namespace lesson._16.cs
{
    public class Util
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
                Console.Write($" {y,2}: ");
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

        static public void Print(AdjancenceVector<double> graph)
        {
            if (graph.NodesCount == 0)
            {
                Console.WriteLine("No nodes\n");
                return;
            }

            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) = (Console.ForegroundColor, Console.BackgroundColor);
            for (int node = 0; node < graph.NodesCount; ++node)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($" {node,2}:");
                Console.ForegroundColor = ConsoleColor.White;
                (int, double)[] adjancentNodes = graph.Data[node];
                if (adjancentNodes.Length > 0)
                    for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                    {
                        (int adjancentNode, double weight) = adjancentNodes[incendence];
                        Console.BackgroundColor = (node & 0x1) == 0 ? ConsoleColor.Black : ConsoleColor.DarkGray;
                        Console.Write($" {adjancentNode,2}, {weight,6:g5} ");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write("|");
                    }
                else
                {
                    Console.BackgroundColor = (node & 0x1) == 0 ? ConsoleColor.Black : ConsoleColor.DarkGray;
                    Console.Write($"            ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("|");
                }
                Console.WriteLine("");
            }
            (Console.ForegroundColor, Console.BackgroundColor) = (foregroundColor, backgroundColor);
            Console.WriteLine("");
        }

        static public void Print(AdjancenceArray<double> graph)
        {
            if (graph.NodesCount == 0)
            {
                Console.WriteLine("No nodes\n");
                return;
            }

            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) = (Console.ForegroundColor, Console.BackgroundColor);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"      |");
            for (int node = 0; node < graph.NodesCount; ++node)
                Console.Write($"{node,6}|");
            Console.Write($"     ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            for (int from = 0; from < graph.NodesCount; ++from)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"{from,6}|");
                Console.ForegroundColor = ConsoleColor.White;
                for (int to = 0; to < graph.NodesCount; ++to)
                {
                    Console.BackgroundColor = (from & 0x1) == 0 ? ConsoleColor.Black : ConsoleColor.DarkGray;
                    if (graph.HasEdge(from, to))
                        Console.Write($"{graph.GetEdgeData(from, to),6:g5}");
                    else
                        Console.Write("      ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("|");
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"{from,-6}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"      |");
            for (int node = 0; node < graph.NodesCount; ++node)
                Console.Write($"{node,6}|");
            Console.Write($"     ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            (Console.ForegroundColor, Console.BackgroundColor) = (foregroundColor, backgroundColor);
            Console.WriteLine("");
        }

        static public void Print(EdgeArray<double> graph)
        {
            if (graph.NodesCount == 0)
            {
                Console.WriteLine("No nodes\n");
                return;
            }
            if (graph.Data.Length == 0)
            {
                Console.WriteLine("No edges\n");
                return;
            }

            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) = (Console.ForegroundColor, Console.BackgroundColor);
            (int from, int to, double weight) = graph.Data[0];
            for (int edge = 0; edge < graph.Data.Length; ++edge)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($" {edge,2}: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = (edge & 0x1) == 0 ? ConsoleColor.Black : ConsoleColor.DarkGray;
                (from, to, weight) = graph.Data[edge];
                Console.Write($"({from,2} -> {to,2}), {weight,6:g5}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine(" |");
            }
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
