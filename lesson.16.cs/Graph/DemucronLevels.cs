using System;

namespace lesson._16.cs
{
    class DemucronLevels
    {
        AdjancenceVector graph;

        int[][] data;

        public int[][] Data { get { Build(); return data; } }

        public DemucronLevels(AdjancenceVector graph)
        {
            this.graph = graph;
            data = null;
        }

        void Build()
        {
            if (data != null)
                return;

            AdjancenceArray adjancenceArray = new AdjancenceArray(graph);

            int[] nodeStocks = new int[graph.NodesCount];
            int usedNodesCount = 0;
            bool[] usedNodesFlag = new bool[graph.NodesCount];

            for (int node = 0; node < graph.NodesCount; ++node)
                for (int adjancentNode = 0; adjancentNode < graph.NodesCount; ++adjancentNode)
                    if (adjancenceArray.Data[node, adjancentNode])
                        ++nodeStocks[adjancentNode];

            NodeStack<NodeStack<int>> skewStack = new NodeStack<NodeStack<int>>();

            while (usedNodesCount < usedNodesFlag.Length)
            {
                skewStack.Push(new NodeStack<int>());

                for (int node = 0; node < graph.NodesCount; ++node)
                    if (!usedNodesFlag[node] && nodeStocks[node] == 0)
                        skewStack.Top.Push(node);
                if (skewStack.Top.size == 0)
                    throw new Exception("Cycles found in graph");

                usedNodesCount += skewStack.Top.size;
                for (Node<int> node = skewStack.Top.top; node != null; node = node.next)
                {
                    usedNodesFlag[node.value] = true;
                    for (int adjancentNode = 0; adjancentNode < graph.NodesCount; ++adjancentNode)
                        if (!usedNodesFlag[adjancentNode] && adjancenceArray.Data[node.value, adjancentNode])
                            --nodeStocks[adjancentNode];
                }
            }

            data = Util.SkewListToArray(skewStack);
        }
    }
}
