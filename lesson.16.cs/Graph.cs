using System;

namespace lesson._16.cs
{
    class Graph
    {
        public enum NodeColors
        {
            White,
            Gray,
            Black,
        };

        public AdjancenceVector Data { get; set; }

        public Graph(AdjancenceVector adjancenceVector)
        {
            Data = adjancenceVector;
        }

        public int[][] Demucron()
        {
            AdjancenceArray adjancenceArray = new AdjancenceArray(Data);

            int[] nodeStocks = new int[Data.NodesCount];
            int usedNodesCount = 0;
            bool[] usedNodesFlag = new bool[Data.NodesCount];

            for (int node = 0; node < Data.NodesCount; ++node)
                for (int adjancentNode = 0; adjancentNode < Data.NodesCount; ++adjancentNode)
                    if (adjancenceArray.Data[node, adjancentNode])
                        ++nodeStocks[adjancentNode];

            NodeStack<NodeStack<int>> skewStack = new NodeStack<NodeStack<int>>();

            while (usedNodesCount < usedNodesFlag.Length)
            {
                skewStack.Push(new NodeStack<int>());

                for (int node = 0; node < Data.NodesCount; ++node)
                    if (!usedNodesFlag[node] && nodeStocks[node] == 0)
                        skewStack.Top.Push(node);
                if (skewStack.Top.size == 0)
                    throw new Exception("Cycles found in graph");

                usedNodesCount += skewStack.Top.size;
                for (Node<int> node = skewStack.Top.top; node != null; node = node.next)
                {
                    usedNodesFlag[node.value] = true;
                    for (int adjancentNode = 0; adjancentNode < Data.NodesCount; ++adjancentNode)
                        if (!usedNodesFlag[adjancentNode] && adjancenceArray.Data[node.value, adjancentNode])
                            --nodeStocks[adjancentNode];
                }
            }

            return Util.SkewListToArray(skewStack);
        }

        // from OTUS lesson's pdf
        //public int[] TarjanRecursive()
        //{
        //    NodeColors[] nodeColors = new NodeColors[Data.NodesCount];

        //    int count = 0;
        //    Node<int> root = null;

        //    if (!TarjanRecursiveStart(ref count, ref root, nodeColors))
        //        throw new Exception("Invalid data for Tarjan");

        //    return Util.ListToArray<int>(count, root);
        //}

        //public bool TarjanRecursiveStart(ref int count, ref Node<int> root, NodeColors[] nodeColors)
        //{
        //    for (int node = 0; node < Data.NodesCount; ++node)
        //        if(nodeColors[node] == NodeColors.White)
        //        {
        //            if (!TarjanRecursiveDSF(node, ref count, ref root, nodeColors))
        //                return false;
        //        }
        //    return true;
        //}

        //public bool TarjanRecursiveDSF(int node, ref int count, ref Node<int> root, NodeColors[] nodeColors)
        //{
        //    nodeColors[node] = NodeColors.Gray;
        //    int[] adjancentNodes = Data.Data[node];
        //    for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
        //    {
        //        int anotherNode = adjancentNodes[incendence];
        //        if (nodeColors[anotherNode] == NodeColors.White)
        //        {
        //            if (!TarjanRecursiveDSF(anotherNode, ref count, ref root, nodeColors))
        //                return false;
        //        } else
        //        {
        //            if (nodeColors[anotherNode] == NodeColors.Gray)
        //                return false;
        //        }
        //    }
        //    nodeColors[node] = NodeColors.Black;
        //    root = new Node<int>(node, root);
        //    ++count;
        //    return true;
        //}

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
            int baseConnectionRank = Tarjan().Length;

            NodeStack<(int, int)> bridgeStack = new NodeStack<(int, int)>();

            for (int node = 0; node < Data.NodesCount; ++node)
            {
                int[] adjancentNodes = Data.Data[node];
                for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                {
                    int adjancentNode = adjancentNodes[incendence];
                    Graph reducedGraph = new Graph(Data.RemoveEdge(node, adjancentNode, directed));
                    if (baseConnectionRank != reducedGraph.Tarjan().Length)
                        bridgeStack.Push((node, adjancentNode));
                }
            }

            return Util.ListToArray<(int, int)>(bridgeStack);
        }
    }
}
