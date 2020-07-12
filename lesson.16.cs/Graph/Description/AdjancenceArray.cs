using System;

namespace lesson._16.cs
{
    public class AdjancenceArray<T>
        where T : struct
    {
        T?[,] data;

        public T?[,] Data { get { return data; } set { Validate(value); data = value; } }
        public int NodesCount { get { return data.GetLength(0); } }

        static void Validate(T?[,] adjancenceArray)
        {
            if (adjancenceArray.GetLength(0) != adjancenceArray.GetLength(1))
                throw new IndexOutOfRangeException();
        }

        public AdjancenceArray(T?[,] adjancenceArray)
        {
            Validate(adjancenceArray);
            data = adjancenceArray;
        }

        public AdjancenceArray(AdjancenceVector<T> adjancenceVector)
        {
            Random rand = new Random();
            int nodes = adjancenceVector.NodesCount;
            data = new T?[nodes, nodes];
            for (int node = 0; node < nodes; ++node)
            {
                (int, T)[] adjancentNodes = adjancenceVector.Data[node];
                for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                {
                    (int adjancentNode, T edgeData) = adjancentNodes[incendence];
                    data[node, adjancentNode] = edgeData;
                }
            }
        }

        public AdjancenceArray(EdgeArray<T> edgeArray)
        {
            data = new T?[edgeArray.NodesCount, edgeArray.NodesCount];
            for (int edge = 0; edge < edgeArray.Data.Length; ++edge)
            {
                (int from, int to, T edgeData) = edgeArray.Data[edge];
                data[from, to] = edgeData;
            }
        }

        public bool HasEdge(int from, int to) { return data[from, to] != null; }
        public T GetEdgeData(int from, int to) { return data[from, to].Value; }

        public AdjancenceArray<T> RemoveNode(int node)
        {
            if (node < 0 || node >= data.Length)
                throw new IndexOutOfRangeException();

            T?[,] adjancenceArray = new T?[data.GetLength(0) - 1, data.GetLength(0) - 1];
            for ((int anotherNode, int offsetNode) = (0, 0); anotherNode < data.GetLength(0); ++anotherNode)
                if (anotherNode != node)
                    for ((int adjancentNode, int offsetAdjancentNode) = (0, 0); adjancentNode < data.GetLength(0); ++adjancentNode)
                        if (adjancentNode != anotherNode)
                            adjancenceArray[anotherNode - offsetNode, adjancentNode - offsetAdjancentNode] = data[anotherNode, adjancentNode];
                        else
                            offsetAdjancentNode = 1;
                else
                    offsetNode = 1;

            return new AdjancenceArray<T>(adjancenceArray);
        }

        public AdjancenceArray<T> RemoveEdge(int from, int to, bool directed = true)
        {
            if (from < 0 || from >= data.Length)
                throw new IndexOutOfRangeException();
            if (to < 0 || to >= data.Length)
                throw new IndexOutOfRangeException();

            T?[,] adjancenceArray = new T?[data.GetLength(0), data.GetLength(0)];
            Array.Copy(data, adjancenceArray, data.Length);
            adjancenceArray[from, to] = null;
            if (!directed)
                adjancenceArray[to, from] = null;

            return new AdjancenceArray<T>(adjancenceArray);
        }

        public AdjancenceArray<T> Unidirectional()
        {
            T?[,] adjancenceArray = new T?[data.GetLength(0), data.GetLength(0)];
            for (int node = 0; node < data.GetLength(0); ++node)
                for (int adjancentNode = 0; adjancentNode < data.GetLength(0); ++adjancentNode)
                {
                    T? edgeData = data[node, adjancentNode];
                    if (edgeData != null)
                    {
                        adjancenceArray[node, adjancentNode] = edgeData;
                        if (adjancenceArray[adjancentNode, node] == null)
                            adjancenceArray[adjancentNode, node] = edgeData;
                    }
                }
            return new AdjancenceArray<T>(adjancenceArray);
        }

        public AdjancenceArray<T> Monodirectional()
        {
            T?[,] adjancenceArray = new T?[data.GetLength(0), data.GetLength(0)];
            for (int node = 0; node < data.GetLength(0); ++node)
                for (int adjancentNode = 0; adjancentNode < data.GetLength(0); ++adjancentNode)
                {
                    T? edgeData = data[node, adjancentNode];
                    if (edgeData != null)
                    {
                        if (adjancenceArray[adjancentNode, node] != null)
                            if (node > adjancentNode)
                                adjancenceArray[adjancentNode, node] = edgeData;
                            else
                                adjancenceArray[node, adjancentNode] = edgeData;
                    }
                }
            return new AdjancenceArray<T>(adjancenceArray);
        }

        public AdjancenceArray<T> Mirror()
        {
            T?[,] adjancenceArray = new T?[data.GetLength(0), data.GetLength(0)];
            for (int node = 0; node < data.GetLength(0); ++node)
                for (int adjancentNode = 0; adjancentNode < data.GetLength(0); ++adjancentNode)
                {
                    T? edgeData = data[node, adjancentNode];
                    if (edgeData != null)
                        adjancenceArray[adjancentNode, node] = edgeData;
                }
            return new AdjancenceArray<T>(adjancenceArray);
        }

    }
}
