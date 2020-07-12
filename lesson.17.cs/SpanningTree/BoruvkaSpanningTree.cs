using lesson._16.cs;
using System;

namespace lesson._17.cs
{
    class BoruvkaSpanningTree<T>
        where T : struct, IComparable
    {
        AdjancenceVector<T> graph;
        EdgeArray<T> data;

        public EdgeArray<T> Data { get { Build(); return data; } }

        public BoruvkaSpanningTree(AdjancenceVector<T> graph)
        {
            this.graph = graph;
            data = null;
        }

        void Build1()
        {
            if (data != null)
                return;

            NodeStack<(int, int, T)> spanningTree = new NodeStack<(int, int, T)>();

            UnionFind nodeRoot = new UnionFind(graph.NodesCount);
            (int, T?)[] nearest = new (int, T?)[graph.NodesCount];


            do
            {
                int foundEdges = 0;
                Array.Fill(nearest, (-1, null));
                for (int node = 0; node < graph.NodesCount; ++node)
                {
                    (int, T)[] adjancentNodes = graph.Data[node];
                    for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                    {
                        (int adjancentNode, T edgeData) = adjancentNodes[incendence];
                        if (!nodeRoot.HasOneRoot(node, adjancentNode))
                            if (nearest[node].Item2 == null || edgeData.CompareTo(nearest[node].Item2.Value) < 0)
                                nearest[node] = (adjancentNode, edgeData);
                    }
                    if (nearest[node].Item2.HasValue)
                        ++foundEdges;
                }
                if (foundEdges == 0)
                    break;
                for (int node = 0; node < graph.NodesCount; ++node)
                    if (nearest[node].Item2 != null)
                    {
                        (int adjancentNode, T? edgeData) = nearest[node];
                        spanningTree.Push((node, adjancentNode, edgeData.Value));
                        nodeRoot.Merge(node, adjancentNode);
                    }
            } while (nodeRoot.Groups > 1);

            data = new EdgeArray<T>(graph.NodesCount, Util.ListToArray<(int, int, T)>(spanningTree));
        }

        void Build()
        {
            if (data != null)
                return;

            NodeStack<(int, int, T)> spanningTree = new NodeStack<(int, int, T)>();

            EdgeArray<T> edgeArray = new EdgeArray<T>(graph);
            UnionFind nodeRoot = new UnionFind(graph.NodesCount);
            int[] nearestEdge = new int[graph.NodesCount];

            int selectedEdges = edgeArray.Data.Length;
            while (selectedEdges > 0)
            {
                Array.Fill(nearestEdge, -1);
                int newSelectedEdges = 0;
                for (int edge = 0; edge < selectedEdges; ++edge)
                {
                    (int from, int to, T edgeData) = edgeArray.Data[edge];
                    (int fromRoot, int toRoot) = (nodeRoot.Find(from), nodeRoot.Find(to));

                    if (fromRoot == toRoot)
                        continue;
                    (edgeArray.Data[edge], edgeArray.Data[newSelectedEdges]) = (edgeArray.Data[newSelectedEdges], edgeArray.Data[edge]);

                    if (nearestEdge[fromRoot] == -1 || edgeData.CompareTo(edgeArray.Data[nearestEdge[fromRoot]].Item3) < 0)
                        nearestEdge[fromRoot] = newSelectedEdges;
                    if (nearestEdge[toRoot] == -1 || edgeData.CompareTo(edgeArray.Data[nearestEdge[toRoot]].Item3) < 0)
                        nearestEdge[toRoot] = newSelectedEdges;

                    ++newSelectedEdges;
                }
                selectedEdges = newSelectedEdges;
                for (int node = 0; node < graph.NodesCount; ++node)
                    if (nearestEdge[node] != -1)
                    {
                        (int from, int to, T edgeData) = edgeArray.Data[nearestEdge[node]];
                        if (!nodeRoot.HasOneRoot(from, to))
                        {
                            spanningTree.Push((from, to, edgeData));
                            nodeRoot.Merge(from, to);
                        }
                    }
            }

            data = new EdgeArray<T>(graph.NodesCount, Util.ListToArray<(int, int, T)>(spanningTree));
        }

    }
}
