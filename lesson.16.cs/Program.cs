using System;

namespace lesson._16.cs
{
    class Program
    {
        static void TestDemucron()
        {
            Console.WriteLine(new String('-', Console.WindowWidth - 1));
            Console.WriteLine("Test Demucron");

            AdjancenceVector adjancenceVector = new AdjancenceVector(new int[][] {
                /*  0 */new int[] { 2, 12 },
                /*  1 */new int[] { 12 },
                /*  2 */new int[] { },
                /*  3 */new int[] { 2 },
                /*  4 */new int[] { 2, 8, 9 },
                /*  5 */new int[] { 3, 10, 11, 12 },
                /*  6 */new int[] { 5, 10 },
                /*  7 */new int[] { 1, 3, 5, 6, },
                /*  8 */new int[] { 0, 13 },
                /*  9 */new int[] { 0, 6, 11},
                /* 10 */new int[] { 2 },
                /* 11 */new int[] { },
                /* 12 */new int[] { 2 },
                /* 13 */new int[] { 10 },
            });
            Console.WriteLine("adjancenceVector");
            adjancenceVector.Print();

            AdjancenceArray adjancenceArray = new AdjancenceArray(adjancenceVector);
            Console.WriteLine("adjancenceArray");
            adjancenceArray.Print();

            Console.WriteLine("Gant's datagramm");
            Util.Print((new DemucronLevels(adjancenceVector)).Data);

            Console.WriteLine("");
        }

        static void TestTarjan()
        {
            Console.WriteLine(new String('-', Console.WindowWidth - 1));
            Console.WriteLine("Test Tarjan strong connected groups search");

            // eng wiki
            //AdjancenceVector adjancenceVector = new AdjancenceVector(new int[][] {
            //    /*  0 */new int[] { 1 },
            //    /*  1 */new int[] { 2 },
            //    /*  2 */new int[] { 0 },
            //    /*  3 */new int[] { 1, 2, 4 },
            //    /*  4 */new int[] { 3, 5 },
            //    /*  5 */new int[] { 2, 6 },
            //    /*  6 */new int[] { 5 },
            //    /*  7 */new int[] { 4, 6, 7 },
            //});
            // lesson
            AdjancenceVector adjancenceVector = new AdjancenceVector(new int[][] {
                /*  0 */new int[] { 1 },
                /*  1 */new int[] { 2, 4, 5 },
                /*  2 */new int[] { 3, 5 },
                /*  3 */new int[] { 2, 7 },
                /*  4 */new int[] { 0, 5 },
                /*  5 */new int[] { 6 },
                /*  6 */new int[] { 5 },
                /*  7 */new int[] { 3, 6 },
            });
            //// lesson reversed edge order
            //AdjancenceVector adjancenceVector = new AdjancenceVector(new int[][] {
            //    /*  0 */new int[] { 1 },
            //    /*  1 */new int[] { 5, 4, 2 },
            //    /*  2 */new int[] { 5, 3 },
            //    /*  3 */new int[] { 7, 2 },
            //    /*  4 */new int[] { 5, 0 },
            //    /*  5 */new int[] { 6 },
            //    /*  6 */new int[] { 5 },
            //    /*  7 */new int[] { 6, 3 },
            //});
            Console.WriteLine("adjancenceVector");
            adjancenceVector.Print();

            Graph graph = new Graph(adjancenceVector);

            Console.WriteLine("Strong Connected Nodes (iterative)");
            Util.Print(graph.Tarjan());

            Console.WriteLine("Strong Connected Nodes (recursive)");
            Util.Print((new TarjanStrongConnected(adjancenceVector)).Data);

            Console.WriteLine("");
        }

        static void TestArticulation()
        {
            Console.WriteLine(new String('-', Console.WindowWidth - 1));
            Console.WriteLine("Test articulation nodes search");

            //AdjancenceVector adjancenceVector = new AdjancenceVector(new int[][] {
            //    /*  0 */new int[] { 1, 2 },
            //    /*  1 */new int[] { 0, 3 },
            //    /*  2 */new int[] { 0, 3 },
            //    /*  3 */new int[] { 1, 2, 4, 5 },
            //    /*  4 */new int[] { 3, 6 },
            //    /*  5 */new int[] { 3, 6 },
            //    /*  6 */new int[] { 4, 5 },
            //});
            // Bridge: 1, 3; from lesson's pdf, but 4 (edgeless node) is articulation point too
            AdjancenceVector adjancenceVector = new AdjancenceVector(new int[][] {
                /*  0 */new int[] { 1, 3 },
                /*  1 */new int[] { 0, 2, 3 },
                /*  2 */new int[] { 1 },
                /*  3 */new int[] { 0, 1, 5, 6 },
                /*  4 */new int[] { },
                /*  5 */new int[] { 3, 6 },
                /*  6 */new int[] { 3, 5 },
            });
            Console.WriteLine("adjancenceVector");
            adjancenceVector.Print();

            Graph graph = new Graph(adjancenceVector);

            Console.WriteLine("Articulation nodes (bruteforce)");
            Util.Print(graph.ArticulationNodes());

            Console.WriteLine("Articulation nodes (recursive)");
            Util.Print((new ArticulationNodes(adjancenceVector)).Data);

            Console.WriteLine("");
        }

        static void TestBridge()
        {
            Console.WriteLine(new String('-', Console.WindowWidth - 1));
            Console.WriteLine("Test bridge edge search");

            //// Bridge: 3 -> 4
            //AdjancenceVector adjancenceVector = new AdjancenceVector(new int[][] {
            //    /*  0 */new int[] { 1, 2 },
            //    /*  1 */new int[] { 0, 3 },
            //    /*  2 */new int[] { 0, 3 },
            //    /*  3 */new int[] { 1, 2, 4},
            //    /*  4 */new int[] { 3, 5, 6 },
            //    /*  5 */new int[] { 4, 7 },
            //    /*  6 */new int[] { 4, 7 },
            //    /*  7 */new int[] { 5, 6 },
            //});
            // Bridge: 2 -> 3, 7 -> 8, 5 -> 9; from lesson's pdf; but what about 0 -> 1, 1 -> 2, etc which create new groups?
            AdjancenceVector adjancenceVector = new AdjancenceVector(new int[][] {
                /*  0 */new int[] { 1, 2 },
                /*  1 */new int[] { 2 },
                /*  2 */new int[] { 0, 3 },
                /*  3 */new int[] { 4, 5, 6, 7 },
                /*  4 */new int[] { 5 },
                /*  5 */new int[] { 3, 9 },
                /*  6 */new int[] { 7 },
                /*  7 */new int[] { 3, 8 },
                /*  8 */new int[] { },
                /*  9 */new int[] { },
            });
            //// the same as above, but dual links
            //AdjancenceVector adjancenceVector = new AdjancenceVector(new int[][] {
            //    /*  0 */new int[] { 1, 2 },
            //    /*  1 */new int[] { 0, 2 },
            //    /*  2 */new int[] { 0, 1, 3 },
            //    /*  3 */new int[] { 2, 4, 5, 6, 7 },
            //    /*  4 */new int[] { 3, 5 },
            //    /*  5 */new int[] { 3, 4, 9 },
            //    /*  6 */new int[] { 3, 7 },
            //    /*  7 */new int[] { 3, 6, 8 },
            //    /*  8 */new int[] { 7 },
            //    /*  9 */new int[] { 5 },
            //});

            Console.WriteLine("adjancenceVector");
            adjancenceVector.Print();

            Graph graph = new Graph(adjancenceVector);

            Console.WriteLine("Strong Connected Nodes (iterative)");
            Util.Print(graph.Tarjan());

            Console.WriteLine("Bridge edges (brootforce) Strong?");
            Util.Print(graph.BridgeEdges());

            Console.WriteLine("Bridge edges (recursive) Weak?");
            Util.Print((new BridgeEdges(adjancenceVector)).Data);

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
