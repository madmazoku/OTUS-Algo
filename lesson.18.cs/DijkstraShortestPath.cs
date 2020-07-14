using lesson._16.cs;
using System;

namespace lesson._18.cs
{
    class DijkstraShortestPath
    {
        AdjancenceVector<double> graph;
        int startNode;
        (int, double, double)[] data;

        public EdgeArray<double> Path(int endNode)
        {
            Build();

            NodeQueue<(int, int, double)> edges = new NodeQueue<(int, int, double)>();
            int node = endNode;
            while (node != startNode)
            {
                (int backNode, double weight, double sumWeight) = data[node];
                edges.Enque((backNode, node, weight));
                node = backNode;
            }

            return new EdgeArray<double>(graph.NodesCount, Util.ListToArray<(int, int, double)>(edges));
        }

        public DijkstraShortestPath(AdjancenceVector<double> graph, int startNode)
        {
            this.graph = graph;
            this.startNode = startNode;
            data = null;
        }

        void Build()
        {
            if (data != null)
                return;

            data = new (int, double, double)[graph.NodesCount];
            Array.Fill(data, (-1, double.MaxValue, double.MaxValue));

            int usedNodesCount = 0;
            bool[] usedNodes = new bool[graph.NodesCount];

            (int minNode, double minWeight) = (startNode, 0);
            data[minNode] = (minNode, minWeight, minWeight);

            while (minWeight < double.MaxValue)
            {
                usedNodes[minNode] = true;
                ++usedNodesCount;

                (int, double)[] adjancentNodes = graph.Data[minNode];
                for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                {
                    (int adjancentNode, double adjancentWeight) = adjancentNodes[incendence];
                    if (adjancentWeight < 0)
                        throw new ArgumentException("negative weight");

                    if (data[minNode].Item3 + adjancentWeight < data[adjancentNode].Item3)
                        data[adjancentNode] = (minNode, adjancentWeight, data[minNode].Item3 + adjancentWeight);
                }

                (minNode, minWeight) = (-1, double.MaxValue);
                for (int node = 0; node < graph.NodesCount; ++node)
                    if (!usedNodes[node] && data[node].Item3 < minWeight)
                        (minNode, minWeight) = (node, data[node].Item3);
            }

            if (usedNodesCount < graph.NodesCount)
                throw new ArgumentException("unaccessable nodes");
        }
    }
}
