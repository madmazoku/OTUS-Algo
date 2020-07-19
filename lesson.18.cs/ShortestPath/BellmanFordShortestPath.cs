using lesson._16.cs;
using System;

namespace lesson._18.cs
{
    class BellmanFordShortestPath
    {
        AdjancenceVector<double> graph;
        (int, double, double)[][] data;

        public EdgeArray<double> Path(int startNode, int endNode)
        {
            Build();

            NodeQueue<(int, int, double)> edges = new NodeQueue<(int, int, double)>();
            int node = endNode;
            while (node != startNode)
            {
                (int backNode, double weight, _) = data[startNode][node];
                if (backNode == -1)
                    throw new ArgumentException("unaccessable nodes");
                edges.Enque((backNode, node, weight));
                node = backNode;
            }

            return new EdgeArray<double>(graph.NodesCount, Util.ListToArray<(int, int, double)>(edges));
        }

        public BellmanFordShortestPath(AdjancenceVector<double> graph)
        {
            this.graph = graph;
            data = null;
        }

        void Build()
        {
            if (data != null)
                return;

            EdgeArray<double> edgeArray = new EdgeArray<double>(graph);

            data = new (int, double, double)[graph.NodesCount][];
            for (int startNode = 0; startNode < graph.NodesCount; ++startNode)
            {
                (int, double, double)[] dataPath = data[startNode] = new (int, double, double)[graph.NodesCount];
                Array.Fill(dataPath, (-1, double.MaxValue, double.MaxValue));
                dataPath[startNode] = (startNode, 0, 0);

                bool relaxed;
                int realxedCount = 0;
                do
                {
                    if (realxedCount++ == graph.NodesCount)
                        throw new ArgumentException("negative loop");
                    relaxed = false;
                    for (int edge = 0; edge < edgeArray.Data.Length; ++edge)
                    {
                        (int from, int to, double weight) = edgeArray.Data[edge];
                        if (dataPath[from].Item1 != -1)
                            if (dataPath[to].Item3 > dataPath[from].Item3 + weight)
                            {
                                dataPath[to] = (from, weight, dataPath[from].Item3 + weight);
                                relaxed = true;
                            }
                    }
                } while (relaxed);
            }
        }
    }
}
