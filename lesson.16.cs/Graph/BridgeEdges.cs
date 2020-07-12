using System;

namespace lesson._16.cs
{
    class BridgeEdges<T>
        where T : struct
    {
        AdjancenceVector<T> graph;

        int order;
        int[] pre;
        int[] low;

        NodeStack<(int, int, T)> stack;

        EdgeArray<T> data;

        public EdgeArray<T> Data { get { Build(); return data; } }

        public BridgeEdges(AdjancenceVector<T> graph)
        {
            this.graph = graph;

            order = 0;
            pre = new int[graph.NodesCount];
            low = new int[graph.NodesCount];

            Array.Fill(pre, -1);

            stack = new NodeStack<(int, int, T)>();

            data = null;
        }

        void Build()
        {
            if (data != null)
                return;

            for (int node = 0; node < graph.NodesCount; ++node)
                if (pre[node] == -1)
                    DSF(node, -1);

            data = new EdgeArray<T>(graph.NodesCount, Util.ListToArray(stack));
        }

        void DSF(int node, int prevNode)
        {
            pre[node] = low[node] = order++;

            (int, T)[] adjancentNodes = graph.Data[node];
            for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
            {
                (int adjancentNode, T edgeData) = adjancentNodes[incendence];
                if (pre[adjancentNode] == -1)
                {
                    DSF(adjancentNode, node);
                    if (low[node] > low[adjancentNode])
                        low[node] = low[adjancentNode];
                    if (low[adjancentNode] == pre[adjancentNode])
                        stack.Push((node, adjancentNode, edgeData));
                }
                else if (adjancentNode != prevNode)
                    if (low[node] > pre[adjancentNode])
                        low[node] = pre[adjancentNode];
            }
        }
    }
}
