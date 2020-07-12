using lesson._16.cs;
using System;

namespace lesson._17.cs
{
    class Program
    {
        static void TestKruskal()
        {
            AdjancenceVector<double> adjancenceVector = new AdjancenceVector<double>(new (int, double)[][] {
                /* 0 A */new (int, double)[] { (1, 7.0), (3, 5.0) },
                /* 1 B */new (int, double)[] { (2, 8.0), (3, 9.0), (4, 7.0) },
                /* 2 C */new (int, double)[] { (4, 5.0) },
                /* 3 D */new (int, double)[] { (4, 15.0), (5, 6.0) },
                /* 4 E */new (int, double)[] { (5, 8.0), (6, 9.0) },
                /* 5 F */new (int, double)[] { (6, 11.0) },
                /* 6 G */new (int, double)[] { },
            });
            Console.WriteLine("adjancenceVector");
            Util.Print(adjancenceVector);

            Console.WriteLine("Kruskal spanning tree");
            EdgeArray<double> edgeArray = (new KruskalSpanningTree<double>(adjancenceVector)).Data;
            QuickSort<(int, int, double)>.Sort(edgeArray.Data, (a, b) => { return a.CompareTo(b); });
            Util.Print(edgeArray);
        }

        static void TestPrim()
        {
            AdjancenceVector<double> adjancenceVector = new AdjancenceVector<double>(new (int, double)[][] {
                /* 0 A */new (int, double)[] { (1, 7.0), (3, 5.0) },
                /* 1 B */new (int, double)[] { (2, 8.0), (3, 9.0), (4, 7.0) },
                /* 2 C */new (int, double)[] { (4, 5.0) },
                /* 3 D */new (int, double)[] { (4, 15.0), (5, 6.0) },
                /* 4 E */new (int, double)[] { (5, 8.0), (6, 9.0) },
                /* 5 F */new (int, double)[] { (6, 11.0) },
                /* 6 G */new (int, double)[] { },
            });
            adjancenceVector = adjancenceVector.Unidirectional();
            Console.WriteLine("adjancenceVector");
            Util.Print(adjancenceVector);

            Console.WriteLine("Prim spanning tree");
            EdgeArray<double> edgeArray = (new PrimSpanningTree<double>(adjancenceVector)).Data;
            QuickSort<(int, int, double)>.Sort(edgeArray.Data, (a, b) => { return a.CompareTo(b); });
            Util.Print(edgeArray);
        }

        static void TestBoruvka()
        {
            AdjancenceVector<double> adjancenceVector = new AdjancenceVector<double>(new (int, double)[][] {
                /* 0 A */new (int, double)[] { (1, 7.0), (3, 5.0) },
                /* 1 B */new (int, double)[] { (2, 8.0), (3, 9.0), (4, 7.0) },
                /* 2 C */new (int, double)[] { (4, 5.0) },
                /* 3 D */new (int, double)[] { (4, 15.0), (5, 6.0) },
                /* 4 E */new (int, double)[] { (5, 8.0), (6, 9.0) },
                /* 5 F */new (int, double)[] { (6, 11.0) },
                /* 6 G */new (int, double)[] { },
            });
            Console.WriteLine("adjancenceVector");
            Util.Print(adjancenceVector);

            Console.WriteLine("Boruvka spanning tree");
            EdgeArray<double> edgeArray = (new BoruvkaSpanningTree<double>(adjancenceVector)).Data;
            QuickSort<(int, int, double)>.Sort(edgeArray.Data, (a, b) => { return a.CompareTo(b); });
            Util.Print(edgeArray);
        }

        static void Main(string[] args)
        {
            TestKruskal();
            TestPrim();
            TestBoruvka();
        }
    }
}
