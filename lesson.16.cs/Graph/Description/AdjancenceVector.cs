using System;

namespace lesson._16.cs
{
    public class AdjancenceVector<T>
        where T : struct
    {
        (int, T)[][] data;

        public (int, T)[][] Data { get { return data; } set { Validate(value); data = value; } }
        public int NodesCount { get { return data.Length; } }

        static void Validate((int, T)[][] adjancenceVector)
        {
            for (int node = 0; node < adjancenceVector.Length; ++node)
                for (int incendence = 0; incendence < adjancenceVector[node].Length; ++incendence)
                {
                    (int adjancentNode, _) = adjancenceVector[node][incendence];
                    if (adjancentNode < 0 || adjancentNode >= adjancenceVector.Length)
                        throw new IndexOutOfRangeException();
                }
        }

        public AdjancenceVector((int, T)[][] adjancenceVector)
        {
            Validate(adjancenceVector);
            data = adjancenceVector;
        }

        public AdjancenceVector(AdjancenceArray<T> adjancenceArray)
        {
            data = new (int, T)[adjancenceArray.NodesCount][];
            for (int node = 0; node < adjancenceArray.NodesCount; ++node)
            {
                NodeStack<(int, T)> stack = new NodeStack<(int, T)>();
                for (int adjancentNode = 0; adjancentNode < adjancenceArray.NodesCount; ++adjancentNode)
                {
                    T? edgeData = adjancenceArray.Data[node, adjancentNode];
                    if (edgeData != null)
                        stack.Push((adjancentNode, edgeData.Value));
                }
                data[node] = Util.ListToArray<(int, T)>(stack);
            }
        }

        public AdjancenceVector(EdgeArray<T> edgeArray)
        {
            NodeStack<(int, T)>[] stacks = new NodeStack<(int, T)>[edgeArray.NodesCount];
            for (int node = 0; node < stacks.Length; ++node)
                stacks[node] = new NodeStack<(int, T)>();
            for (int edge = 0; edge < edgeArray.Data.Length; ++edge)
            {
                (int from, int to, T edgeData) = edgeArray.Data[edge];
                stacks[from].Push((to, edgeData));
            }
            data = new (int, T)[stacks.Length][];
            for (int node = 0; node < stacks.Length; ++node)
                data[node] = Util.ListToArray<(int, T)>(stacks[node]);
        }

        public AdjancenceVector<T> RemoveNode(int node)
        {
            if (node < 0 || node >= data.Length)
                throw new IndexOutOfRangeException();

            NodeStack<NodeStack<(int, T)>> skewStack = new NodeStack<NodeStack<(int, T)>>();

            for (int anotherNode = 0; anotherNode < data.Length; ++anotherNode)
                if (anotherNode != node)
                {
                    skewStack.Push(new NodeStack<(int, T)>());

                    (int, T)[] adjancentNodes = data[anotherNode];
                    for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                    {
                        (int adjancentNode, T edgeData) = adjancentNodes[incendence];
                        if (adjancentNode != node)
                            skewStack.Top.Push((adjancentNode < node ? adjancentNode : adjancentNode - 1, edgeData));
                    }

                }

            return new AdjancenceVector<T>(Util.SkewListToArray(skewStack));
        }

        public AdjancenceVector<T> RemoveEdge(int from, int to, bool directed = true)
        {
            if (from < 0 || from >= data.Length)
                throw new IndexOutOfRangeException();
            if (to < 0 || to >= data.Length)
                throw new IndexOutOfRangeException();

            (int, T)[][] adjancenceVector = new (int, T)[data.Length][];
            for (int node = 0; node < data.Length; ++node)
            {
                (int, T)[] adjancentNodes = data[node];
                if (node != from && (directed || node != to))
                {
                    adjancenceVector[node] = new (int, T)[adjancentNodes.Length];
                    Array.Copy(adjancentNodes, adjancenceVector[node], adjancentNodes.Length);
                }
                else
                {
                    NodeStack<(int, T)> stack = new NodeStack<(int, T)>();
                    for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                    {
                        (int adjancentNode, T edgeData) = adjancentNodes[incendence];
                        if (adjancentNode != to && (directed || adjancentNode != from))
                            stack.Push((adjancentNode, edgeData));
                    }
                    adjancenceVector[node] = Util.ListToArray<(int, T)>(stack);
                }
            }

            return new AdjancenceVector<T>(adjancenceVector);
        }

        public AdjancenceVector<T> Unidirectional()
        {
            NodeStack<(int, T)>[] nodeStacks = new NodeStack<(int, T)>[data.Length];
            for (int node = 0; node < data.Length; ++node)
                nodeStacks[node] = new NodeStack<(int, T)>();

            for (int node = 0; node < data.Length; ++node)
            {
                (int, T)[] adjancentNodes = data[node];
                for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                {
                    (int adjancentNode, T edgeData) = adjancentNodes[incendence];
                    nodeStacks[node].Push((adjancentNode, edgeData));

                    if (!nodeStacks[adjancentNode].Find((x) => { return x.Item1 == node; }))
                        nodeStacks[adjancentNode].Push((node, edgeData));
                }
            }

            (int, T)[][] adjancenceVector = new (int, T)[data.Length][];
            for (int node = 0; node < data.Length; ++node)
                adjancenceVector[node] = Util.ListToArray<(int, T)>(nodeStacks[node]);

            return new AdjancenceVector<T>(adjancenceVector);
        }

        public AdjancenceVector<T> Monodirectional()
        {
            NodeStack<(int, T)>[] nodeStacks = new NodeStack<(int, T)>[data.Length];
            for (int node = 0; node < data.Length; ++node)
                nodeStacks[node] = new NodeStack<(int, T)>();

            for (int node = 0; node < data.Length; ++node)
            {
                (int, T)[] adjancentNodes = data[node];
                for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                {
                    (int adjancentNode, T edgeData) = adjancentNodes[incendence];
                    if (!nodeStacks[adjancentNode].Find((x) => { return x.Item1 == node; }))
                        if (node > adjancentNode)
                            nodeStacks[adjancentNode].Push((node, edgeData));
                        else
                            nodeStacks[node].Push((adjancentNode, edgeData));
                }
            }

            (int, T)[][] adjancenceVector = new (int, T)[data.Length][];
            for (int node = 0; node < data.Length; ++node)
                adjancenceVector[node] = Util.ListToArray<(int, T)>(nodeStacks[node]);

            return new AdjancenceVector<T>(adjancenceVector);
        }

        public AdjancenceVector<T> Mirror()
        {
            NodeStack<(int, T)>[] nodeStacks = new NodeStack<(int, T)>[data.Length];
            for (int node = 0; node < data.Length; ++node)
                nodeStacks[node] = new NodeStack<(int, T)>();

            for (int node = 0; node < data.Length; ++node)
            {
                (int, T)[] adjancentNodes = data[node];
                for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                {
                    (int adjancentNode, T edgeData) = adjancentNodes[incendence];
                    nodeStacks[adjancentNode].Push((node, edgeData));
                }
            }

            (int, T)[][] adjancenceVector = new (int, T)[data.Length][];
            for (int node = 0; node < data.Length; ++node)
                adjancenceVector[node] = Util.ListToArray<(int, T)>(nodeStacks[node]);

            return new AdjancenceVector<T>(adjancenceVector);
        }
    }
}
