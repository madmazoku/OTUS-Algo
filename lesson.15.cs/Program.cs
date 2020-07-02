using System;
using System.Linq;

namespace lesson._15.cs
{
    class Program
    {
        static int[] MinSFPath(Graph g, int from, int to)
        {
            int[] minPath = null;
            Func<NodeList<int>, bool> collectorPredicat = (path) =>
            {
                if (path.Size > 1 && path.Tail.Value == to && (minPath == null || minPath.Length > path.Size))
                    minPath = path.Values.ToArray();
                return false;
            };
            g.BFS(from, collectorPredicat);
            return minPath;
        }

        static void Main(string[] args)
        {
            AdjacenceArray aa = new AdjacenceArray(7);
            aa.AddEdges(0, new[] { 1, 2 }, false);
            aa.AddEdges(1, new[] { 3, 5 }, false);
            aa.AddEdges(2, new[] { 5, 4, 6 }, false);
            aa.AddEdges(3, new[] { 5, 4 }, false);
            aa.AddEdges(4, new[] { 6 }, false);

            int[][] array = aa.Build();

            Console.WriteLine("Graph, AdjacenceArray:");
            for (int node = 0; node < array.Length; ++node)
            {
                Console.Write($"{node,4}:");
                for (int adjance = 0; adjance < array[node].Length; ++adjance)
                    Console.Write($" {array[node][adjance],3};");
                Console.WriteLine("");
            }
            Console.WriteLine("\n");

            Graph g = new Graph(array);
            NodeList<int[]> pathes = new NodeList<int[]>();

            Console.WriteLine("Iterate pathes:");
            int pathCount = 0;

            g.BFS(0, (path) =>
            {
                Console.Write($"[{pathCount++,3}]:");
                foreach (int node in path.Values)
                    Console.Write($" {node,3};");
                Console.WriteLine("");
                if (path.Tail.Value == 0)
                {
                    int[] newPath = path.Values.ToArray();
                    pathes.InsertIf(newPath, (x, y) => { return x.Length > y.Length; });
                }
                return false;
            });
            Console.WriteLine("");

            Console.WriteLine("All pathes from 0 to 0");
            pathCount = 0;
            foreach (int[] path in pathes.Values)
            {
                Console.Write($"[{pathCount++,3}]:");
                foreach (int node in path)
                    Console.Write($" {node,3};");
                Console.WriteLine("");
            }
            Console.WriteLine("");

            for (int i = 0; i < array.Length; ++i)
                for (int j = 0; j < array.Length; ++j)
                {
                    Console.Write($"[{i,3} => {j,-3}]:");
                    int[] path = MinSFPath(g, i, j);
                    if (path != null)
                        foreach (int node in path)
                            Console.Write($" {node};");
                    else
                        Console.Write(" No path found");
                    Console.WriteLine("");
                }
            Console.WriteLine("");

        }
    }
}
