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

            int countLevels = 0;
            Node<Node<int>> rootLevel = null;
            Node<int> rootLevelSize = null;

            while (usedNodesCount < usedNodesFlag.Length)
            {
                ++countLevels;
                rootLevel = new Node<Node<int>>(null, rootLevel);
                rootLevelSize = new Node<int>(0, rootLevelSize);

                for (int node = 0; node < Data.NodesCount; ++node)
                    if (!usedNodesFlag[node] && nodeStocks[node] == 0)
                    {
                        rootLevel.value = new Node<int>(node, rootLevel.value);
                        ++rootLevelSize.value;
                    }
                if (rootLevelSize.value == 0)
                    throw new Exception("Cycles found in graph");

                usedNodesCount += rootLevelSize.value;
                for (Node<int> node = rootLevel.value; node != null; node = node.next)
                {
                    usedNodesFlag[node.value] = true;
                    for (int adjancentNode = 0; adjancentNode < Data.NodesCount; ++adjancentNode)
                        if (!usedNodesFlag[adjancentNode] && adjancenceArray.Data[node.value, adjancentNode])
                            --nodeStocks[adjancentNode];
                }
            }

            return Util.SkewListToArray(countLevels, rootLevel, rootLevelSize);
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

        // from https://www.intuit.ru/studies/courses/12181/1174/lecture/25266?page=11

        class TarjanState
        {
            public Graph graph;
            public Node<int> S;
            public int cnt;
            public int scnt;
            public int[] pre;
            public int[] low;
            public int[] id;

            public TarjanState(Graph graph)
            {
                this.graph = graph;

                S = null;
                cnt = 0;
                scnt = 0;
                pre = new int[graph.Data.NodesCount];
                Array.Fill(pre, -1);
                low = new int[graph.Data.NodesCount];
                id = new int[graph.Data.NodesCount];
            }
        }

        public int[][] TarjanRecursive()
        {
            TarjanState state = new TarjanState(this);
            for (int node = 0; node < Data.NodesCount; ++node)
                if (state.pre[node] == -1)
                    TarjanRecursive(node, state);

            int[] countStrongConnectedArray = new int[state.scnt];
            Node<int>[] rootStrongConnectedArray = new Node<int>[state.scnt];
            for (int node = 0; node < Data.NodesCount; ++node)
            {
                ++countStrongConnectedArray[state.id[node]];
                rootStrongConnectedArray[state.id[node]] = new Node<int>(node, rootStrongConnectedArray[state.id[node]]);
            }

            int[][] strongConnected = new int[state.scnt][];
            for (int id = 0; id < state.scnt; ++id)
                strongConnected[id] = Util.ListToArray<int>(countStrongConnectedArray[id], rootStrongConnectedArray[id]);

            return strongConnected;
        }

        void TarjanRecursive(int w, TarjanState state)
        {
            int t;
            int min = state.low[w] = state.pre[w] = state.cnt++;
            state.S = new Node<int>(w, state.S);

            int[] adjancentNodes = state.graph.Data.Data[w];
            for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
            {
                t = adjancentNodes[incendence];
                if (state.pre[t] == -1)
                    TarjanRecursive(t, state);
                if (state.low[t] < min)
                    min = state.low[t];
            }
            if (min < state.low[w])
            {
                state.low[w] = min;
                return;
            }
            do
            {
                t = state.S.value;
                state.S = state.S.next;
                state.id[t] = state.scnt;
                state.low[t] = state.graph.Data.NodesCount;
            } while (t != w);
            ++state.scnt;
        }

        public int[][] Tarjan()
        {
            int countLevels = 0;
            Node<Node<int>> rootLevel = null;
            Node<int> rootLevelSize = null;

            NodeColors[] nodeColors = new NodeColors[Data.NodesCount];
            int[] low = new int[Data.NodesCount];
            Node<int> stack = null;
            int cnt = 0;

            for (int initialNode = 0; initialNode < Data.NodesCount; ++initialNode)
            {
                if (nodeColors[initialNode] != NodeColors.White)
                    continue;

                Node<int> rootNodeFrame = new Node<int>(initialNode, null);
                Node<int> rootIncendenceFrame = new Node<int>(Data.Data[initialNode].Length, null);
                Node<int> rootMinFrame = new Node<int>(low[initialNode] = cnt++, null);
                nodeColors[initialNode] = NodeColors.Gray;
                stack = new Node<int>(initialNode, stack);

                while (rootNodeFrame != null)
                {
                    while (rootIncendenceFrame.value > 0)
                    {
                        --rootIncendenceFrame.value;
                        int adjancentNode = Data.Data[rootNodeFrame.value][rootIncendenceFrame.value];
                        if (nodeColors[adjancentNode] == NodeColors.White)
                        {
                            rootNodeFrame = new Node<int>(adjancentNode, rootNodeFrame);
                            rootIncendenceFrame = new Node<int>(Data.Data[adjancentNode].Length, rootIncendenceFrame);
                            rootMinFrame = new Node<int>(low[adjancentNode] = cnt++, rootMinFrame);
                            nodeColors[adjancentNode] = NodeColors.Gray;
                            stack = new Node<int>(adjancentNode, stack);
                        }
                        else if (nodeColors[adjancentNode] == NodeColors.Gray)
                        {
                            if (rootMinFrame.value > low[adjancentNode])
                                rootMinFrame.value = low[adjancentNode];
                        }
                    }
                    while (rootNodeFrame != null && rootIncendenceFrame.value == 0)
                    {
                        if (low[rootNodeFrame.value] > rootMinFrame.value)
                            low[rootNodeFrame.value] = rootMinFrame.value;
                        else
                        {
                            ++countLevels;
                            rootLevel = new Node<Node<int>>(null, rootLevel);
                            rootLevelSize = new Node<int>(0, rootLevelSize);
                            do
                            {
                                Util.MoveNodeBetweenLists<int>(ref stack, ref rootLevel.value);
                                ++rootLevelSize.value;
                                nodeColors[rootLevel.value.value] = NodeColors.Black;
                            } while (rootLevel.value.value != rootNodeFrame.value);
                            Util.ReverseList<int>(ref rootLevel.value);
                        }
                        rootNodeFrame = rootNodeFrame.next;
                        rootIncendenceFrame = rootIncendenceFrame.next;
                        rootMinFrame = rootMinFrame.next;
                    }
                    if (rootNodeFrame != null)
                    {
                        int adjancentNode = Data.Data[rootNodeFrame.value][rootIncendenceFrame.value];
                        if (rootMinFrame.value > low[adjancentNode])
                            rootMinFrame.value = low[adjancentNode];
                    }
                }
            }

            return Util.SkewListToArray<int>(countLevels, rootLevel, rootLevelSize);
        }

        int[] ArticulationNodes()
        {
            int baseConnectionRank = Tarjan().Length;

            int countArticulationNode = 0;
            Node<int> rootArticulationNode = null;

            for (int node = 0; node < Data.NodesCount; ++node)
            {
                Graph reducedGraph = new Graph(Data.RemoveNode(node));
                if (baseConnectionRank != reducedGraph.Tarjan().Length)
                {
                    ++countArticulationNode;
                    rootArticulationNode = new Node<int>(node, rootArticulationNode);
                }

            }

            return Util.ListToArray<int>(countArticulationNode, rootArticulationNode);
        }

        (int, int)[] BridgeEdges()
        {
            int baseConnectionRank = Tarjan().Length;

            int countBridgeEdge = 0;
            Node<(int, int)> rootBridgeEdge = null;

            for (int node = 0; node < Data.NodesCount; ++node)
            {
                int[] adjancentNodes = Data.Data[node];
                for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                {
                    int adjancentNode = adjancentNodes[incendence];
                    Graph reducedGraph = new Graph(Data.RemoveEdge(node, adjancentNode));
                    if (baseConnectionRank != reducedGraph.Tarjan().Length)
                    {
                        ++countBridgeEdge;
                        rootBridgeEdge = new Node<(int, int)>((node, adjancentNode), rootBridgeEdge);
                    }

                }
            }

            return Util.ListToArray<(int, int)>(countBridgeEdge, rootBridgeEdge);
        }
    }
}
