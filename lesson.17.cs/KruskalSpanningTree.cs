using lesson._16.cs;
using System;

namespace lesson._17.cs
{
    class KruskalSpanningTree<T>
        where T : struct, IComparable
    {
        AdjancenceVector<T> graph;
        EdgeArray<T> data;

        public EdgeArray<T> Data { get { Build(); return data; } }

        public KruskalSpanningTree(AdjancenceVector<T> graph)
        {
            this.graph = graph;
            data = null;
        }

        void Build()
        {
            if (data != null)
                return;

            EdgeArray<T> edgeArray = new EdgeArray<T>(graph);
            QuickSort<(int, int, T)>.Sort(edgeArray.Data, (a, b) => { return a.Item3.CompareTo(b.Item3); });

            NodeStack<(int, int, T)> spanningTree = new NodeStack<(int, int, T)>();
            UnionFind nodeRoot = new UnionFind(graph.NodesCount);

            for (int edge = 0; edge < edgeArray.Data.Length; ++edge)
            {
                (int from, int to, T edgeData) = edgeArray.Data[edge];
                if (!nodeRoot.HasOneRoot(from, to))
                {
                    spanningTree.Push((from, to, edgeData));
                    nodeRoot.Merge(from, to);
                    if (spanningTree.size == graph.NodesCount - 1)
                        break;
                }
            }

            data = new EdgeArray<T>(graph.NodesCount, Util.ListToArray<(int, int, T)>(spanningTree));
        }
    }
}
