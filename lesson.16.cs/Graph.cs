using System;
using System.Diagnostics;

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
            if(min < state.low[w])
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

        class TarjanFrame
        {
            public int node;
            public int incendence;
            public int min;

            public TarjanFrame(int node)
            {
                this.node = node;
            }
            public TarjanFrame(int node, int incendence, int min)
            {
                this.node = node;
                this.incendence = incendence;
                this.min = min;
            }
        }

        void PrintFrames(string text, Node<TarjanFrame> rootFrame)
        {
            Console.Write($"{text,25}:");
            if (rootFrame == null)
                Console.Write(" Empty;");
            else
                for ((Node<TarjanFrame> node, int index) = (rootFrame, 0); node != null; node = node.next, ++index)
                    Console.Write($" {index,2}: (node: {node.value.node,2}; min: {node.value.min}; inc: {node.value.incendence});");
            Console.WriteLine();
        }

        

        public int[][] Tarjan()
        {
            int[] pre = new int[Data.NodesCount];
            int[] low = new int[Data.NodesCount];
            int[] id = new int[Data.NodesCount];
            Node<int> stack = null;
            int cnt = 0;
            int scnt = 0;
            
            Array.Fill(pre, -1);

            for (int initialNode = 0; initialNode < Data.NodesCount; ++initialNode)
            {
                if (pre[initialNode] != -1)
                    continue;

                Node<TarjanFrame> rootFrame = new Node<TarjanFrame>(new TarjanFrame(initialNode), null);
                rootFrame.value.incendence = Data.Data[rootFrame.value.node].Length;
                rootFrame.value.min = pre[rootFrame.value.node] = low[rootFrame.value.node] = cnt++;
                stack = new Node<int>(rootFrame.value.node, stack);

                while(rootFrame != null)
                {
                    while(rootFrame.value.incendence > 0)
                    {
                        --rootFrame.value.incendence;
                        int adjancentNode = Data.Data[rootFrame.value.node][rootFrame.value.incendence];
                        if (pre[adjancentNode] == -1)
                        {
                            rootFrame = new Node<TarjanFrame>(new TarjanFrame(adjancentNode), rootFrame);
                            rootFrame.value.incendence = Data.Data[rootFrame.value.node].Length;
                            rootFrame.value.min = pre[rootFrame.value.node] = low[rootFrame.value.node] = cnt++;
                            stack = new Node<int>(rootFrame.value.node, stack);
                        } else
                        {
                            if (rootFrame.value.min > low[adjancentNode])
                                rootFrame.value.min = low[adjancentNode];
                        }
                    }
                    while (rootFrame != null && rootFrame.value.incendence == 0)
                    {
                        if (low[rootFrame.value.node] > rootFrame.value.min)
                            low[rootFrame.value.node] = rootFrame.value.min;
                        else
                        {
                            int node;
                            do
                            {
                                node = stack.value;
                                stack = stack.next;
                                id[node] = scnt;
                                low[node] = Data.NodesCount;
                            } while (node != rootFrame.value.node);
                            ++scnt;
                        }
                        rootFrame = rootFrame.next;
                        if (rootFrame != null)
                        {
                            int adjancentNode = Data.Data[rootFrame.value.node][rootFrame.value.incendence];
                            if (rootFrame.value.min > low[adjancentNode])
                                rootFrame.value.min = low[adjancentNode];
                        }
                    }
                }

            }

            int[] countStrongConnectedArray = new int[scnt];
            Node<int>[] rootStrongConnectedArray = new Node<int>[scnt];
            for (int node = 0; node < Data.NodesCount; ++node)
            {
                ++countStrongConnectedArray[id[node]];
                rootStrongConnectedArray[id[node]] = new Node<int>(node, rootStrongConnectedArray[id[node]]);
            }

            int[][] strongConnected = new int[scnt][];
            for (int sid = 0; sid < scnt; ++sid)
                strongConnected[sid] = Util.ListToArray<int>(countStrongConnectedArray[sid], rootStrongConnectedArray[sid]);

            return strongConnected;
        }

        //public int[][] Tarjan()
        //{
        //    int blackNodesCount = 0;
        //    NodeColors[] nodesColor = new NodeColors[Data.NodesCount];
        //    int countNodeOrder = 0;
        //    Node<int> rootNodeOrder = null;

        //    // topological sort
        //    while (blackNodesCount < nodesColor.Length)
        //    {
        //        int countNodePath = 0;
        //        Node<int> rootNodePath = null;
        //        Node<int> incendencePath = null;
        //        for (int node = 0; node < nodesColor.Length; ++node)
        //            if (nodesColor[node] == NodeColors.White)
        //            {
        //                nodesColor[node] = NodeColors.Gray;
        //                rootNodePath = new Node<int>(node, rootNodePath);
        //                incendencePath = new Node<int>(Data.Data[node].Length, incendencePath);
        //                ++countNodePath;
        //                break;
        //            }

        //        // DFS
        //        while (rootNodePath != null)
        //        {
        //            while (incendencePath.value > 0)
        //            {
        //                --incendencePath.value;
        //                int node = Data.Data[rootNodePath.value][incendencePath.value];
        //                if (nodesColor[node] == NodeColors.White)
        //                {
        //                    nodesColor[node] = NodeColors.Gray;
        //                    rootNodePath = new Node<int>(node, rootNodePath);
        //                    incendencePath = new Node<int>(Data.Data[node].Length, incendencePath);
        //                    ++countNodePath;
        //                    break;
        //                }
        //            }

        //            if (incendencePath.value > 0)
        //                continue;

        //            while (incendencePath != null && incendencePath.value == 0)
        //            {
        //                nodesColor[rootNodePath.value] = NodeColors.Black;
        //                ++blackNodesCount;

        //                Util.MoveNodeBetweenLists<int>(ref rootNodePath, ref rootNodeOrder);
        //                incendencePath = incendencePath.next;
        //                ++countNodeOrder;
        //                --countNodePath;
        //            }

        //        }
        //    }

        //    Util.ReverseList<int>(ref rootNodeOrder);

        //    int countLevels = 0;
        //    Node<Node<int>> rootLevel = null;
        //    Node<int> rootLevelSize = null;

        //    // sub graphs extract
        //    while (blackNodesCount > 0)
        //    {
        //        Node<int> nodePath = null;
        //        Node<int> incendencePath = null;
        //        while (nodesColor[rootNodeOrder.value] != NodeColors.Black)
        //            rootNodeOrder = rootNodeOrder.next;

        //        ++countLevels;
        //        rootLevel = new Node<Node<int>>(null, rootLevel);
        //        rootLevelSize = new Node<int>(0, rootLevelSize);

        //        nodesColor[rootNodeOrder.value] = NodeColors.Gray;
        //        Util.MoveNodeBetweenLists<int>(ref rootNodeOrder, ref nodePath);
        //        incendencePath = new Node<int>(Data.Data[nodePath.value].Length, incendencePath);

        //        // DFS
        //        while (nodePath != null)
        //        {
        //            while (incendencePath.value > 0)
        //            {
        //                --incendencePath.value;
        //                int node = Data.Data[nodePath.value][incendencePath.value];
        //                if (nodesColor[node] == NodeColors.Black)
        //                {
        //                    nodesColor[node] = NodeColors.Gray;
        //                    nodePath = new Node<int>(node, nodePath);
        //                    incendencePath = new Node<int>(Data.Data[node].Length, incendencePath);
        //                    break;
        //                }
        //            }

        //            if (incendencePath.value > 0)
        //                continue;

        //            while (incendencePath != null && incendencePath.value == 0)
        //            {
        //                nodesColor[nodePath.value] = NodeColors.White;
        //                --blackNodesCount;

        //                Util.MoveNodeBetweenLists<int>(ref nodePath, ref rootLevel.value);
        //                incendencePath = incendencePath.next;
        //                ++rootLevelSize.value;
        //            }
        //        }
        //    }

        //    return Util.SkewListToArray(countLevels, rootLevel, rootLevelSize);
        //}

        int[] ArticulationNodes()
        {
            int baseConnectionRank = Tarjan().Length;

            int countArticulationNode = 0;
            Node<int> rootArticulationNode = null;

            for (int node = 0; node < Data.NodesCount; ++node)
            {
                Graph reducedGraph = new Graph(Data.RemoveNode(node));
                if(baseConnectionRank != reducedGraph.Tarjan().Length)
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
