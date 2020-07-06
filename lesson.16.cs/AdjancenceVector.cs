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
                Node<int> root = null;
                int adjancentNodes = 0;
                for (int adjancentNode = 0; adjancentNode < adjancenceArray.NodesCount; ++adjancentNode)
                    if (adjancenceArray.Data[node, adjancentNode])
                    {
                        root = new Node<int>(adjancentNode, root);
                        ++adjancentNodes;
                    }
                data[node] = new int[adjancentNodes];
                while (adjancentNodes > 0)
                {
                    --adjancentNodes;
                    data[node][adjancentNodes] = root.value;
                    root = root.next;
                }
            }
        }

        public AdjancenceVector RemoveNode(int node)
        {
            if (node < 0 || node >= data.Length)
                throw new IndexOutOfRangeException();

            int countLevels = 0;
            Node<Node<int>> rootLevel = null;
            Node<int> rootLevelSize = null;

            for (int anotherNode = 0; anotherNode < data.Length; ++anotherNode)
                if (anotherNode != node)
                {
                    ++countLevels;
                    rootLevel = new Node<Node<int>>(null, rootLevel);
                    rootLevelSize = new Node<int>(0, rootLevelSize);

                    int[] adjancentNodes = data[anotherNode];
                    for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                        if (adjancentNodes[incendence] != node)
                        {
                            rootLevel.value = new Node<int>(adjancentNodes[incendence], rootLevel.value);
                            ++rootLevelSize.value;
                        }

                }

            return new AdjancenceVector(Util.SkewListToArray<int>(countLevels, rootLevel, rootLevelSize));
        }

        public AdjancenceVector RemoveEdge(int from, int to)
        {
            if (from < 0 || from >= data.Length)
                throw new IndexOutOfRangeException();
            if (to < 0 || to >= data.Length)
                throw new IndexOutOfRangeException();

            int[][] adjancenceVector = new int[data.Length][];
            for (int node = 0; node < data.Length; ++node) { 
                int[] adjancentNodes = data[node];
                if (node != from)
                {
                    adjancenceVector[node] = new int[adjancentNodes.Length];
                    Array.Copy(adjancentNodes, adjancenceVector[node], adjancentNodes.Length);
                }
                else
                {
                    int countAdjanceNode = 0;
                    Node<int> rootAdjanceNode = null;
                    for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                        if (adjancentNodes[incendence] != to)
                        {
                            ++countAdjanceNode;
                            rootAdjanceNode = new Node<int>(adjancentNodes[incendence], rootAdjanceNode);
                        }
                    adjancenceVector[node] = Util.ListToArray<int>(countAdjanceNode, rootAdjanceNode);
                }
            }

            return new AdjancenceVector(adjancenceVector);
        }

        public void Print() { Util.Print(data); }
    }
}
