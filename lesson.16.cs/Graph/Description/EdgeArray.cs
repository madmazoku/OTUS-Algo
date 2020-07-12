using System;

namespace lesson._16.cs
{
    public class EdgeArray<T>
        where T : struct
    {
        (int, int, T)[] data;

        public (int, int, T)[] Data { get { return data; } set { Validate(value); data = value; } }
        public int NodesCount { get; set; }

        void Validate((int, int, T)[] edgeArray)
        {
            for (int edge = 0; edge < edgeArray.Length; ++edge)
            {
                (int from, int to, _) = edgeArray[edge];
                if (from < 0 || from > NodesCount)
                    throw new IndexOutOfRangeException();
                if (to < 0 || to > NodesCount)
                    throw new IndexOutOfRangeException();
            }
        }

        public EdgeArray(int nodesCount, (int, int, T)[] edgeArray)
        {
            NodesCount = nodesCount;
            Validate(edgeArray);
            data = edgeArray;
        }

        public EdgeArray(AdjancenceVector<T> adjancenceVector)
        {
            NodesCount = adjancenceVector.NodesCount;
            NodeStack<(int, int, T)> edges = new NodeStack<(int, int, T)>();
            for (int node = 0; node < adjancenceVector.NodesCount; ++node)
            {
                (int, T)[] adjancentNodes = adjancenceVector.Data[node];
                for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                {
                    (int adjancentNode, T edgeData) = adjancentNodes[incendence];
                    edges.Push((node, adjancentNode, edgeData));
                }
            }
            data = Util.ListToArray<(int, int, T)>(edges);
        }

        public EdgeArray(AdjancenceArray<T> adjancenceArray)
        {
            NodesCount = adjancenceArray.NodesCount;
            NodeStack<(int, int, T)> edges = new NodeStack<(int, int, T)>();
            for (int from = 0; from < adjancenceArray.NodesCount; ++from)
                for (int to = 0; to < adjancenceArray.NodesCount; ++to)
                    if (adjancenceArray.HasEdge(from, to))
                        edges.Push((from, to, adjancenceArray.GetEdgeData(from, to)));
        }

        public EdgeArray<T> RemoveNode(int node)
        {
            NodeStack<(int, int, T)> edges = new NodeStack<(int, int, T)>();
            for (int edge = 0; edge < data.Length; ++edge)
            {
                (int from, int to, T edgeData) = data[edge];
                if (from != node && to != node)
                    edges.Push((from < node ? from : from - 1, to < node ? to : to - 1, edgeData));
            }
            return new EdgeArray<T>(NodesCount - 1, Util.ListToArray<(int, int, T)>(edges));
        }

        public EdgeArray<T> RemoveEdge(int from, int to, bool directed = true)
        {
            NodeStack<(int, int, T)> edges = new NodeStack<(int, int, T)>();
            for (int edge = 0; edge < data.Length; ++edge)
            {
                (int fromEdge, int toEdge, T edgeData) = data[edge];
                if (from != fromEdge && to != toEdge && (directed || (from != toEdge && to != fromEdge)))
                    edges.Push((fromEdge, toEdge, edgeData));
            }
            return new EdgeArray<T>(NodesCount, Util.ListToArray<(int, int, T)>(edges));
        }

        public EdgeArray<T> Unidirectional()
        {
            NodeStack<(int, int, T)> edges = new NodeStack<(int, int, T)>();
            for (int edge = 0; edge < data.Length; ++edge)
            {
                (int from, int to, T edgeData) = data[edge];
                edges.Push((from, to, edgeData));
                if (!edges.Find((x) => { return x.Item1 == to && x.Item2 == from; }))
                    edges.Push((to, from, edgeData));
            }
            return new EdgeArray<T>(NodesCount - 1, Util.ListToArray<(int, int, T)>(edges));
        }

        public EdgeArray<T> Monodirectional()
        {
            NodeStack<(int, int, T)> edges = new NodeStack<(int, int, T)>();
            for (int edge = 0; edge < data.Length; ++edge)
            {
                (int from, int to, T edgeData) = data[edge];
                edges.Push((from, to, edgeData));
                if (!edges.Find((x) => { return x.Item1 == to && x.Item2 == from; }))
                    edges.Push((to, from, edgeData));
            }
            return new EdgeArray<T>(NodesCount - 1, Util.ListToArray<(int, int, T)>(edges));
        }

        public EdgeArray<T> Mirror()
        {
            NodeStack<(int, int, T)> edges = new NodeStack<(int, int, T)>();
            for (int edge = 0; edge < data.Length; ++edge)
            {
                (int from, int to, T edgeData) = data[edge];
                edges.Push((to, from, edgeData));
            }
            return new EdgeArray<T>(NodesCount - 1, Util.ListToArray<(int, int, T)>(edges));
        }
    }
}
