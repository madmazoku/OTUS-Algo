using System;

namespace lesson._16.cs
{
    class AdjancenceVector
    {
        int[][] data;

        public int[][] Data { get { return data; } set { Validate(value); data = value; } }
        public int NodesCount { get { return data.Length; } }

        static void Validate(int[][] adjancenceVector)
        {
            for (int node = 0; node < adjancenceVector.Length; ++node)
                for (int incendence = 0; incendence < adjancenceVector[node].Length; ++incendence)
                {
                    int adjancentNode = adjancenceVector[node][incendence];
                    if (adjancentNode < 0 || adjancentNode >= adjancenceVector.Length)
                        throw new IndexOutOfRangeException();
                }
        }

        public AdjancenceVector(int[][] adjancenceVector)
        {
            Data = adjancenceVector;
        }

        public AdjancenceVector(AdjancenceArray adjancenceArray)
        {
            data = new int[adjancenceArray.NodesCount][];
            for (int node = 0; node < adjancenceArray.NodesCount; ++node)
            {
                NodeStack<int> stack = new NodeStack<int>();
                for (int adjancentNode = 0; adjancentNode < adjancenceArray.NodesCount; ++adjancentNode)
                    if (adjancenceArray.Data[node, adjancentNode])
                        stack.Push(adjancentNode);
                data[node] = Util.ListToArray(stack);
            }
        }

        public AdjancenceVector RemoveNode(int node)
        {
            if (node < 0 || node >= data.Length)
                throw new IndexOutOfRangeException();

            NodeStack<NodeStack<int>> skewStack = new NodeStack<NodeStack<int>>();

            for (int anotherNode = 0; anotherNode < data.Length; ++anotherNode)
                if (anotherNode != node)
                {
                    skewStack.Push(new NodeStack<int>());

                    int[] adjancentNodes = data[anotherNode];
                    for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                    {
                        int adjancentNode = adjancentNodes[incendence];
                        if (adjancentNode < node)
                            skewStack.Top.Push(adjancentNode);
                        else if (adjancentNode > node)
                            skewStack.Top.Push(adjancentNode - 1);
                    }

                }

            return new AdjancenceVector(Util.SkewListToArray(skewStack));
        }

        public AdjancenceVector RemoveEdge(int from, int to, bool directed = true)
        {
            if (from < 0 || from >= data.Length)
                throw new IndexOutOfRangeException();
            if (to < 0 || to >= data.Length)
                throw new IndexOutOfRangeException();

            int[][] adjancenceVector = new int[data.Length][];
            for (int node = 0; node < data.Length; ++node)
            {
                int[] adjancentNodes = data[node];
                if (node != from && (directed || node != to))
                {
                    adjancenceVector[node] = new int[adjancentNodes.Length];
                    Array.Copy(adjancentNodes, adjancenceVector[node], adjancentNodes.Length);
                }
                else
                {
                    NodeStack<int> stack = new NodeStack<int>();
                    for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                    {
                        int adjancentNode = adjancentNodes[incendence];
                        if (adjancentNode != to && (directed || adjancentNode != from))
                            stack.Push(adjancentNode);
                    }
                    adjancenceVector[node] = Util.ListToArray<int>(stack);
                }
            }

            return new AdjancenceVector(adjancenceVector);
        }

        public void Print() { Util.Print(data); }
    }
}
