using System;

namespace lesson._16.cs
{
    class BridgeEdges
    {
        AdjancenceVector graph;

        int order;
        int[] pre;
        int[] low;

        NodeStack<(int, int)> stack;

        (int, int)[] data;

        public (int, int)[] Data { get { Build(); return data; } }

        public BridgeEdges(AdjancenceVector graph)
        {
            this.graph = graph;

            order = 0;
            pre = new int[graph.NodesCount];
            low = new int[graph.NodesCount];

            Array.Fill(pre, -1);

            stack = new NodeStack<(int, int)>();

            data = null;
        }

        void Build()
        {
            if (data != null)
                return;

            for (int node = 0; node < graph.NodesCount; ++node)
                if (pre[node] == -1)
                    DSF(node, -1);

            data = Util.ListToArray(stack);
        }

        void DSF(int node, int prevNode)
        {
            pre[node] = low[node] = order++;

            int[] adjancentNodes = graph.Data[node];
            for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
            {
                int adjancentNode = adjancentNodes[incendence];
                if (pre[adjancentNode] == -1)
                {
                    DSF(adjancentNode, node);
                    if (low[node] > low[adjancentNode])
                        low[node] = low[adjancentNode];
                    if (low[adjancentNode] == pre[adjancentNode])
                        stack.Push((node, adjancentNode));
                }
                else if (adjancentNode != prevNode)
                    if (low[node] > pre[adjancentNode])
                        low[node] = pre[adjancentNode];
            }
        }
    }
}
