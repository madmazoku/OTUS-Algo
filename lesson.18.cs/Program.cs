using lesson._16.cs;
using System;

namespace lesson._18.cs
{
    class Program
    {
        static void TestDijkstra()
        {
            Console.WriteLine(new String('-', Console.WindowWidth - 1));
            Console.WriteLine("Test Dijkstra algorithm");

            AdjancenceVector<double> adjancenceVector = new AdjancenceVector<double>(new (int, double)[][] {
                /* 0 */new (int, double)[] { (1, 7.0), (2, 9.0), (5, 14.0) },
                /* 1 */new (int, double)[] { (2, 10.0), (3, 15.0) },
                /* 2 */new (int, double)[] { (3, 11.0), (5, 2.0) },
                /* 3 */new (int, double)[] { (4, 6.0) },
                /* 4 */new (int, double)[] { (5, 9.0) },
                /* 5 */new (int, double)[] { },
            });
            adjancenceVector = adjancenceVector.Unidirectional();
            Console.WriteLine("adjancenceVector");
            Util.Print(adjancenceVector);

            (int startNode, int endNode) = (0, 4);
            Console.WriteLine($"Shortest path from {startNode} to {endNode}");
            EdgeArray<double> edgeArray = (new DijkstraShortestPath(adjancenceVector)).Path(startNode, endNode);
            Util.Print(edgeArray);
        }

        static void TestFloydWarshall()
        {
            Console.WriteLine(new String('-', Console.WindowWidth - 1));
            Console.WriteLine("Test Floyd-Warshall algorithm");

            AdjancenceVector<double> adjancenceVector = new AdjancenceVector<double>(new (int, double)[][] {
                /* 0 */new (int, double)[] { (1, 7.0), (2, 9.0), (5, 14.0) },
                /* 1 */new (int, double)[] { (2, 10.0), (3, 15.0) },
                /* 2 */new (int, double)[] { (3, 11.0), (5, 2.0) },
                /* 3 */new (int, double)[] { (4, 6.0) },
                /* 4 */new (int, double)[] { (5, 9.0) },
                /* 5 */new (int, double)[] { },
            });
            adjancenceVector = adjancenceVector.Unidirectional();
            Console.WriteLine("adjancenceVector");
            Util.Print(adjancenceVector);

            (int startNode, int endNode) = (0, 4);
            Console.WriteLine($"Shortest path from {startNode} to {endNode}");
            EdgeArray<double> edgeArray = (new FloydWarshallShortestPath(adjancenceVector)).Path(startNode, endNode);
            Util.Print(edgeArray);
        }

        static void Main(string[] args)
        {
            TestDijkstra();
            TestFloydWarshall();
        }
    }
}
