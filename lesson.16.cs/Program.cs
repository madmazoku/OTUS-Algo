using System;

namespace lesson._16.cs
{
    class Program
    {
        static void TestDemucron()
        {
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

            Graph graph = new Graph(adjancenceVector);

            int[][] grantDatagramm = graph.Demucron();
            Console.WriteLine("Grant's datagramm");
            Util.Print(grantDatagramm);
        }

        //static void TestTarjan()
        //{
        //    Console.WriteLine("Test Tarjan");

        //    //AdjancenceVector adjancenceVector = new AdjancenceVector(new int[][] {
        //    //    /*  0 */new int[] { },
        //    //    /*  1 */new int[] { 3 },
        //    //    /*  2 */new int[] { 0 },
        //    //    /*  3 */new int[] { 0, 2 },
        //    //});
        //    //AdjancenceVector adjancenceVector = new AdjancenceVector(new int[][] {
        //    //    /*  0 */new int[] { 2, 12 },
        //    //    /*  1 */new int[] { 12 },
        //    //    /*  2 */new int[] { },
        //    //    /*  3 */new int[] { 2 },
        //    //    /*  4 */new int[] { 2, 8, 9 },
        //    //    /*  5 */new int[] { 3, 10, 11, 12 },
        //    //    /*  6 */new int[] { 5, 10 },
        //    //    /*  7 */new int[] { 1, 3, 5, 6, },
        //    //    /*  8 */new int[] { 0, 13 },
        //    //    /*  9 */new int[] { 0, 6, 11},
        //    //    /* 10 */new int[] { 2 },
        //    //    /* 11 */new int[] { },
        //    //    /* 12 */new int[] { 2 },
        //    //    /* 13 */new int[] { 10 },
        //    //});
        //    AdjancenceVector adjancenceVector = new AdjancenceVector(new int[][] {
        //            /*  0 */new int[] { 1 },
        //            /*  1 */new int[] { 2, 4, 5 },
        //            /*  2 */new int[] { 3, 5 },
        //            /*  3 */new int[] { 2, 7 },
        //            /*  4 */new int[] { 0, 5 },
        //            /*  5 */new int[] { 6 },
        //            /*  6 */new int[] { 5 },
        //            /*  7 */new int[] { 3, 6 },
        //        });

        //    Console.WriteLine("adjancenceVector");
        //    adjancenceVector.Print();

        //    Graph graph = new Graph(adjancenceVector);

        //    int[] sortedNodesIterative = graph.Tarjan();
        //    Console.WriteLine("Topologically sorted nodes (iterative)");
        //    Util.Print(sortedNodesIterative);

        //    int[] sortedNodesRecursive = graph.TarjanRecursive();
        //    Console.WriteLine("Topologically sorted nodes (recursive)");
        //    Util.Print(sortedNodesRecursive);
        //}


        static void TestTarjan()
        {
            Console.WriteLine("Test Tarjan");

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
            //// lesson reversed
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

            int[][] strongConnectedRecursive = graph.TarjanRecursive();
            Console.WriteLine("Strong Connected Nodes (recursive)");
            Util.Print(strongConnectedRecursive);

            int[][] strongConnectedIterative = graph.Tarjan();
            Console.WriteLine("Strong Connected Nodes (iterative)");
            Util.Print(strongConnectedIterative);
        }

        static void Main(string[] args)
        {
            // TestDemucron();
            TestTarjan();
        }
    }
}
