using System;

namespace lesson._16.cs
{
    class AdjancenceArray
    {
        bool[,] data;

        public bool[,] Data { get { return data; } set { Validate(value); data = value; } }
        public int NodesCount { get { return data.GetLength(0); } }

        static void Validate(bool[,] adjancenceArray)
        {
            if (adjancenceArray.GetLength(0) != adjancenceArray.GetLength(1))
                throw new IndexOutOfRangeException();
        }

        public AdjancenceArray(bool[,] adjancenceArray)
        {
            data = adjancenceArray;
        }

        public AdjancenceArray(AdjancenceVector adjancenceVector)
        {
            int nodes = adjancenceVector.NodesCount;
            data = new bool[nodes, nodes];
            for (int node = 0; node < nodes; ++node)
            {
                int[] adjancentNodes = adjancenceVector.Data[node];
                for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                    data[node, adjancentNodes[incendence]] = true;
            }
        }

        public AdjancenceArray RemoveNode(int node)
        {
            if (node < 0 || node >= data.Length)
                throw new IndexOutOfRangeException();

            bool[,] adjancenceArray = new bool[data.GetLength(0) - 1, data.GetLength(1) - 1];
            for ((int anotherNode, int offsetNode) = (0, 0); anotherNode < data.GetLength(0); ++anotherNode)
                if (anotherNode != node)
                    for ((int adjancentNode, int offsetAdjancentNode) = (0, 0); adjancentNode < data.GetLength(1); ++adjancentNode)
                        if (adjancentNode != anotherNode)
                            adjancenceArray[anotherNode - offsetNode, adjancentNode - offsetAdjancentNode] = data[anotherNode, adjancentNode];
                        else
                            offsetAdjancentNode = 1;
                 else 
                    offsetNode = 1;

            return new AdjancenceArray(adjancenceArray);
        }

        public AdjancenceArray RemoveEdge(int from, int to)
        {
            if (from < 0 || from >= data.Length)
                throw new IndexOutOfRangeException();
            if (to < 0 || to >= data.Length)
                throw new IndexOutOfRangeException();

            bool[,] adjancenceArray = new bool[data.GetLength(0), data.GetLength(1)];
            Array.Copy(data, adjancenceArray, data.Length);
            adjancenceArray[from, to] = false;

            return new AdjancenceArray(adjancenceArray);
        }

        public void Print() { Util.Print(data); }
    }
}
