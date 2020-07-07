using System;

namespace lesson._16.cs
{
    class Graph
    {
        public AdjancenceVector Data { get; set; }

        public Graph(AdjancenceVector adjancenceVector)
        {
            Data = adjancenceVector;
        }

        // from OTUS lesson's pdf, topological sort, not search for strong connected
        public enum NodeColors
        {
            White,
            Gray,
            Black,
        };

        public int[] TarjanRecursive()
        {
            NodeColors[] nodeColors = new NodeColors[Data.NodesCount];

            NodeStack<int> stack = new NodeStack<int>();

            if (!TarjanRecursiveStart(stack, nodeColors))
                throw new Exception("Invalid data for Tarjan");

            return Util.ListToArray<int>(stack);
        }

        public bool TarjanRecursiveStart(NodeStack<int> stack, NodeColors[] nodeColors)
        {
            for (int node = 0; node < Data.NodesCount; ++node)
                if (nodeColors[node] == NodeColors.White)
                {
                    if (!TarjanRecursiveDSF(node, stack, nodeColors))
                        return false;
                }
            return true;
        }

        public bool TarjanRecursiveDSF(int node, NodeStack<int> stack, NodeColors[] nodeColors)
        {
            nodeColors[node] = NodeColors.Gray;
            int[] adjancentNodes = Data.Data[node];
            for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
            {
                int anotherNode = adjancentNodes[incendence];
                if (nodeColors[anotherNode] == NodeColors.White)
                {
                    if (!TarjanRecursiveDSF(anotherNode, stack, nodeColors))
                        return false;
                }
                else
                {
                    if (nodeColors[anotherNode] == NodeColors.Gray)
                        return false;
                }
            }
            nodeColors[node] = NodeColors.Black;
            stack.Push(node);
            return true;
        }

        // iterative Tarjan search for strongly connected 
        public int[][] Tarjan()
        {
            NodeStack<NodeQueue<int>> skewStackQueue = new NodeStack<NodeQueue<int>>();

            NodeColors[] nodeColors = new NodeColors[Data.NodesCount];
            int[] low = new int[Data.NodesCount];
            NodeStack<int> stack = new NodeStack<int>();
            int order = 0;

            NodeStack<int> nodeStack = new NodeStack<int>();
            NodeStack<int> incendenceStack = new NodeStack<int>();
            NodeStack<int> minStack = new NodeStack<int>();

            for (int initialNode = 0; initialNode < Data.NodesCount; ++initialNode)
            {
                if (nodeColors[initialNode] != NodeColors.White)
                    continue;

                nodeStack.Push(initialNode);
                incendenceStack.Push(Data.Data[initialNode].Length);
                minStack.Push(low[initialNode] = order++);
                nodeColors[initialNode] = NodeColors.Gray;
                stack.Push(initialNode);

                while (nodeStack.size > 0)
                {
                    while (incendenceStack.Top > 0)
                    {
                        --incendenceStack.Top;
                        int adjancentNode = Data.Data[nodeStack.Top][incendenceStack.Top];
                        if (nodeColors[adjancentNode] == NodeColors.White)
                        {
                            nodeStack.Push(adjancentNode);
                            incendenceStack.Push(Data.Data[adjancentNode].Length);
                            minStack.Push(low[adjancentNode] = order++);
                            nodeColors[adjancentNode] = NodeColors.Gray;
                            stack.Push(adjancentNode);
                        }
                        else if (nodeColors[adjancentNode] == NodeColors.Gray)
                        {
                            if (minStack.Top > low[adjancentNode])
                                minStack.Top = low[adjancentNode];
                        }
                    }
                    while (nodeStack.size > 0 && incendenceStack.Top == 0)
                    {
                        if (low[nodeStack.Top] > minStack.Top)
                            low[nodeStack.Top] = minStack.Top;
                        else
                        {
                            skewStackQueue.Push(new NodeQueue<int>());
                            do
                            {
                                skewStackQueue.Top.Enque(stack.Pop());
                                nodeColors[skewStackQueue.Top.Last] = NodeColors.Black;
                            } while (skewStackQueue.Top.Last != nodeStack.Top);
                        }
                        nodeStack.Pop();
                        incendenceStack.Pop();
                        minStack.Pop();
                    }
                    if (nodeStack.size > 0)
                    {
                        int adjancentNode = Data.Data[nodeStack.Top][incendenceStack.Top];
                        if (minStack.Top > low[adjancentNode])
                            minStack.Top = low[adjancentNode];
                    }
                }
            }

            return Util.SkewListToArray(skewStackQueue);
        }

        public int[] ArticulationNodes()
        {
            int baseConnectionRank = Tarjan().Length;

            NodeStack<int> articulationStack = new NodeStack<int>();

            for (int node = 0; node < Data.NodesCount; ++node)
            {
                Graph reducedGraph = new Graph(Data.RemoveNode(node));
                if (baseConnectionRank != reducedGraph.Tarjan().Length)
                    articulationStack.Push(node);
            }

            return Util.ListToArray(articulationStack);
        }

        public (int, int)[] BridgeEdges(bool directed = true)
        {
            int[][] baseStrongConnection = Tarjan();
            int baseConnectionRank = baseStrongConnection.Length;

            NodeStack<(int, int)> bridgeStack = new NodeStack<(int, int)>();

            for (int node = 0; node < Data.NodesCount; ++node)
            {
                int[] adjancentNodes = Data.Data[node];
                for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                {
                    int adjancentNode = adjancentNodes[incendence];

                    Graph reducedGraph = new Graph(Data.RemoveEdge(node, adjancentNode, directed));

                    int[][] strongConnection = reducedGraph.Tarjan();
                    if (baseConnectionRank != strongConnection.Length)
                        bridgeStack.Push((node, adjancentNode));
                }
            }

            return Util.ListToArray<(int, int)>(bridgeStack);
        }

    }
}
