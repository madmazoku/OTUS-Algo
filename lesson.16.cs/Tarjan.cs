using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;

// Based on https://www.intuit.ru/studies/courses/12181/1174/lecture/25266?page=11

namespace lesson._16.cs
{
    class Tarjan
    {
        Graph graph;

        NodeStack<int> stack;
        int order;
        int[] pre;
        int[] low;
        NodeStack<NodeQueue<int>> skewStackQueue;

        int[][] strongConnected;

        public int[][] StrongConnected { get { BuildStrongConnected(); return strongConnected; } }

        public Tarjan(Graph graph)
        {
            this.graph = graph;

            stack = new NodeStack<int>();
            order = 0;
            pre = new int[graph.Data.NodesCount];
            low = new int[graph.Data.NodesCount];

            Array.Fill(pre, -1);

            skewStackQueue = new NodeStack<NodeQueue<int>>();

            strongConnected = null;
        }

        void BuildStrongConnected()
        {
            if (strongConnected != null)
                return;

            for (int node = 0; node < graph.Data.NodesCount; ++node)
                if (pre[node] == -1)
                    BuildStrongConnected(node);

            strongConnected = Util.SkewListToArray(skewStackQueue);
        }

        void BuildStrongConnected(int node)
        {
            int min = pre[node] = low[node] = order++;
            stack.Push(node);

            int[] adjancentNodes = graph.Data.Data[node];
            for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
            {
                int adjancentNode = adjancentNodes[incendence];
                if (pre[adjancentNode] == -1)
                    BuildStrongConnected(adjancentNode);
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
                low[skewStackQueue.Top.Last] = graph.Data.NodesCount;
            } while (skewStackQueue.Top.Last != node);
        }
    }
}
