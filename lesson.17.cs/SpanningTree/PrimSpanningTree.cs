using lesson._16.cs;
using System;

namespace lesson._17.cs
{
    class PrimSpanningTree<T>
        where T : struct, IComparable
    {
        AdjancenceVector<T> graph;
        EdgeArray<T> data;

        public EdgeArray<T> Data { get { Build(); return data; } }

        public PrimSpanningTree(AdjancenceVector<T> graph)
        {
            this.graph = graph;
            data = null;
        }

        void Build()
        {
            if (data != null)
                return;

            NodeStack<(int, int, T)> spanningTree = new NodeStack<(int, int, T)>();

            int usedNodesCount = 0;
            bool[] usedNodes = new bool[graph.NodesCount];
            int[] incendences = new int[graph.NodesCount];
            NodeStack<int> nodes = new NodeStack<int>();

            for (int node = 0; node < graph.NodesCount; ++node)
            {
                (int, T)[] adjancentNodes = graph.Data[node];
                if (adjancentNodes.Length == 0)
                {
                    usedNodes[node] = true;
                    ++usedNodesCount;
                    continue;
                }
                QuickSort<(int, T)>.Sort(adjancentNodes, (a, b) => { return a.Item2.CompareTo(b.Item2); });
                incendences[node] = 0;
            }

            while (usedNodesCount < graph.NodesCount)
            {
                (int minNode, T? minEdgeData) = (-1, null);
                for (int node = 0; node < graph.NodesCount; ++node)
                    if (!usedNodes[node])
                    {
                        (int adjancentNode, T edgeData) = graph.Data[node][incendences[node]];
                        if (minEdgeData == null || minEdgeData.Value.CompareTo(edgeData) > 0)
                            (minNode, minEdgeData) = (node, edgeData);
                    }

                nodes.Push(minNode);
                usedNodes[minNode] = true;
                ++usedNodesCount;

                while (nodes.size > 0)
                {
                    int node = nodes.Pop();
                    (int, T)[] adjancentNodes = graph.Data[node];
                    (int adjancentNode, T edgeData) = adjancentNodes[incendences[node]];

                    if (!usedNodes[adjancentNode])
                    {
                        spanningTree.Push((node, adjancentNode, edgeData));
                        usedNodes[adjancentNode] = true;
                        ++usedNodesCount;
                        if (incendences[adjancentNode] < adjancentNodes.Length)
                            nodes.InsertIf(adjancentNode, (x) => { return edgeData.CompareTo(graph.Data[x][incendences[x]].Item2) < 0; });
                    }

                    while (++incendences[node] < adjancentNodes.Length)
                        if (!usedNodes[adjancentNodes[incendences[node]].Item1])
                        {
                            (adjancentNode, edgeData) = adjancentNodes[incendences[node]];
                            nodes.InsertIf(node, (x) => { return edgeData.CompareTo(graph.Data[x][incendences[x]].Item2) < 0; });
                            break;
                        }
                }
            }

            data = new EdgeArray<T>(graph.NodesCount, Util.ListToArray<(int, int, T)>(spanningTree));
        }
    }
}
