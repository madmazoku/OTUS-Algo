using System;

namespace lesson._16.cs
{
    class TarjanStrongConnected
    {
        AdjancenceVector graph;

        int order;
        int[] pre;
        int[] low;
        NodeStack<int> stack;

        NodeStack<NodeQueue<int>> skewStackQueue;

        int[][] data;

        public int[][] Data { get { Build(); return data; } }

        public TarjanStrongConnected(AdjancenceVector graph)
        {
            this.graph = graph;

            order = 0;
            pre = new int[graph.NodesCount];
            low = new int[graph.NodesCount];
            stack = new NodeStack<int>();

            Array.Fill(pre, -1);

            skewStackQueue = new NodeStack<NodeQueue<int>>();

            data = null;
        }

        void Build()
        {
            if (data != null)
                return;

            for (int node = 0; node < graph.NodesCount; ++node)
                if (pre[node] == -1)
                    DSF(node);

            data = Util.SkewListToArray(skewStackQueue);
        }

        void DSF(int node)
        {
            int min = pre[node] = low[node] = order++;
            stack.Push(node);

            int[] adjancentNodes = graph.Data[node];
            for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
            {
                int adjancentNode = adjancentNodes[incendence];
                if (pre[adjancentNode] == -1)
                    DSF(adjancentNode);
                if (min > low[adjancentNode])
                    min = low[adjancentNode];
            }
            if (low[node] > min)
            {
                low[node] = min;
                return;
            }

            skewStackQueue.Push(new NodeQueue<int>());
            do
            {
                skewStackQueue.Top.Enque(stack.Pop());
                low[skewStackQueue.Top.Last] = int.MaxValue;
            } while (skewStackQueue.Top.Last != node);
        }
    }
}
