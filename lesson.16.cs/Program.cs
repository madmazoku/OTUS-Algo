using System;

namespace lesson._16.cs
{
    class Program
    {
        static void TestDemucron()
        {
            Console.WriteLine(new String('-', Console.WindowWidth - 1));
            Console.WriteLine("Test Demucron");

            AdjancenceVector<double> adjancenceVector = new AdjancenceVector<double>(new (int, double)[][] {
                /*  0 */new (int, double)[] { (2, 1.0), (12, 1.0) },
                /*  1 */new (int, double)[] { (12, 1.0) },
                /*  2 */new (int, double)[] { },
                /*  3 */new (int, double)[] { (2, 1.0) },
                /*  4 */new (int, double)[] { (2, 1.0), (8, 1.0), (9, 1.0) },
                /*  5 */new (int, double)[] { (3, 1.0), (10, 1.0), (11, 1.0), (12, 1.0) },
                /*  6 */new (int, double)[] { (5, 1.0), (10, 1.0) },
                /*  7 */new (int, double)[] { (1, 1.0), (3, 1.0), (5, 1.0), (6, 1.0), },
                /*  8 */new (int, double)[] { (0, 1.0), (13, 1.0) },
                /*  9 */new (int, double)[] { (0, 1.0), (6, 1.0), (11, 1.0)},
                /* 10 */new (int, double)[] { (2, 1.0) },
                /* 11 */new (int, double)[] { },
                /* 12 */new (int, double)[] { (2, 1.0) },
                /* 13 */new (int, double)[] { (10, 1.0) },
            });
            Console.WriteLine("adjancenceVector");
            Util.Print(adjancenceVector);

            AdjancenceArray<double> adjancenceArray = new AdjancenceArray<double>(adjancenceVector);
            Console.WriteLine("adjancenceArray");
            Util.Print(adjancenceArray);

            EdgeArray<double> edgeArray = new EdgeArray<double>(adjancenceVector);
            Console.WriteLine("edgeArray");
            Util.Print(edgeArray);

            Console.WriteLine("Gant's datagramm");
            Util.Print((new DemucronLevels<double>(adjancenceVector)).Data);

            Console.WriteLine("");
        }

        static void TestTarjan()
        {
            Console.WriteLine(new String('-', Console.WindowWidth - 1));
            Console.WriteLine("Test Tarjan strong connected groups search");

            // lesson
            AdjancenceVector<double> adjancenceVector = new AdjancenceVector<double>(new (int, double)[][] {
                /*  0 */new (int, double)[] { (1, 1.0) },
                /*  1 */new (int, double)[] { (2, 1.0), (4, 1.0), (5, 1.0) },
                /*  2 */new (int, double)[] { (3, 1.0), (5, 1.0) },
                /*  3 */new (int, double)[] { (2, 1.0), (7, 1.0) },
                /*  4 */new (int, double)[] { (0, 1.0), (5, 1.0) },
                /*  5 */new (int, double)[] { (6, 1.0) },
                /*  6 */new (int, double)[] { (5, 1.0) },
                /*  7 */new (int, double)[] { (3, 1.0), (6, 1.0) },
            });

            Console.WriteLine("adjancenceVector");
            Util.Print(adjancenceVector);

            Graph<double> graph = new Graph<double>(adjancenceVector);

            Console.WriteLine("Strong Connected Nodes (iterative)");
            Util.Print(graph.Tarjan());

            Console.WriteLine("Strong Connected Nodes (recursive)");
            Util.Print((new TarjanStrongConnected<double>(adjancenceVector)).Data);

            Console.WriteLine("");
        }

        static void TestArticulation()
        {
            Console.WriteLine(new String('-', Console.WindowWidth - 1));
            Console.WriteLine("Test articulation nodes search");

            // Bridge: 1, 3; from lesson's pdf, but 4 (edgeless node) is articulation point too
            AdjancenceVector<double> adjancenceVector = new AdjancenceVector<double>(new (int, double)[][] {
                /*  0 */new (int, double)[] { (1, 1.0), (3, 1.0) },
                /*  1 */new (int, double)[] { (0, 1.0), (2, 1.0), (3, 1.0) },
                /*  2 */new (int, double)[] { (1, 1.0) },
                /*  3 */new (int, double)[] { (0, 1.0), (1, 1.0), (5, 1.0), (6, 1.0) },
                /*  4 */new (int, double)[] { },
                /*  5 */new (int, double)[] { (3, 1.0), (6, 1.0) },
                /*  6 */new (int, double)[] { (3, 1.0), (5, 1.0) },
            });
            Console.WriteLine("adjancenceVector");
            Util.Print(adjancenceVector);

            Graph<double> graph = new Graph<double>(adjancenceVector);

            Console.WriteLine("Articulation nodes (bruteforce)");
            Util.Print(graph.ArticulationNodes());

            Console.WriteLine("Articulation nodes (recursive)");
            Util.Print((new ArticulationNodes<double>(adjancenceVector)).Data);

            Console.WriteLine("");
        }

        static void TestBridge()
        {
            Console.WriteLine(new String('-', Console.WindowWidth - 1));
            Console.WriteLine("Test bridge edge search");

            // Bridge: 2 -> 3, 7 -> 8, 5 -> 9; from lesson's pdf; but what about 0 -> 1, 1 -> 2, etc which create new groups?
            AdjancenceVector<double> adjancenceVector = new AdjancenceVector<double>(new (int, double)[][] {
                /*  0 */new (int, double)[] { (1, 1.0), (2, 1.0) },
                /*  1 */new (int, double)[] { (2, 1.0) },
                /*  2 */new (int, double)[] { (0, 1.0), (3, 1.0) },
                /*  3 */new (int, double)[] { (4, 1.0), (5, 1.0), (6, 1.0), (7, 1.0) },
                /*  4 */new (int, double)[] { (5, 1.0) },
                /*  5 */new (int, double)[] { (3, 1.0), (9, 1.0) },
                /*  6 */new (int, double)[] { (7, 1.0) },
                /*  7 */new (int, double)[] { (3, 1.0), (8, 1.0) },
                /*  8 */new (int, double)[] { },
                /*  9 */new (int, double)[] { },
            });

            Console.WriteLine("adjancenceVector");
            Util.Print(adjancenceVector);

            Graph<double> graph = new Graph<double>(adjancenceVector);

            Console.WriteLine("Strong Connected Nodes (iterative)");
            Util.Print(graph.Tarjan());

            Console.WriteLine("Bridge edges (brootforce) Strong?");
            Util.Print(graph.BridgeEdges());

            Console.WriteLine("Bridge edges (recursive) Weak?");
            Util.Print((new BridgeEdges<double>(adjancenceVector)).Data);

            Console.WriteLine("");
        }

        static void Main(string[] args)
        {
            TestDemucron();
            TestTarjan();
            TestArticulation();
            TestBridge();
        }
    }
}
