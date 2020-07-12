using System;

namespace lesson._16.cs
{
    class ArticulationNodes<T>
        where T : struct
    {
        AdjancenceVector<T> graph;

        int order;
        int[] pre;
        int[] low;

        public NodeStack<int> stack;

        int[] data;

        public int[] Data { get { Build(); return data; } }

        public ArticulationNodes(AdjancenceVector<T> graph)
        {
            this.graph = graph;

            order = 0;
            pre = new int[graph.NodesCount];
            low = new int[graph.NodesCount];

            Array.Fill(pre, -1);

            stack = new NodeStack<int>();

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
            int count = 0;
            int max = 0;

            pre[node] = low[node] = order++;

            (int, T)[] adjancentNodes = graph.Data[node];
            for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
            {
                (int adjancentNode, _) = adjancentNodes[incendence];
                if (pre[adjancentNode] == -1)
                {
                    DSF(adjancentNode, node);
                    if (low[node] > low[adjancentNode])
                        low[node] = low[adjancentNode];

                    ++count;
                    if (max < low[adjancentNode])
                        max = low[adjancentNode];
                }
                else if (adjancentNode != prevNode)
                    if (low[node] > pre[adjancentNode])
                        low[node] = pre[adjancentNode];
            }
            if ((prevNode != -1 && max >= pre[node]) || (prevNode == -1 && (adjancentNodes.Length == 0 || count > 1)))
                stack.Push(node);
        }
    }
}
