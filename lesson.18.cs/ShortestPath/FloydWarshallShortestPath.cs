using lesson._16.cs;
using System;

namespace lesson._18.cs
{
    class FloydWarshallShortestPath
    {
        AdjancenceVector<double> graph;
        (int, double, double)[,] data;

        public EdgeArray<double> Path(int startNode, int endNode)
        {
            Build();

            NodeQueue<(int, int, double)> edges = new NodeQueue<(int, int, double)>();
            int node = endNode;
            while (node != startNode)
            {
                (int backNode, double weight, _) = data[startNode, node];
                if (backNode == -1)
                    throw new ArgumentException("unaccessable nodes");
                edges.Enque((backNode, node, weight));
                node = backNode;
            }

            return new EdgeArray<double>(graph.NodesCount, Util.ListToArray<(int, int, double)>(edges));
        }

        public FloydWarshallShortestPath(AdjancenceVector<double> graph)
        {
            this.graph = graph;
            data = null;
        }

        void Build()
        {
            if (data != null)
                return;

            data = new (int, double, double)[graph.NodesCount, graph.NodesCount];
            for (int node = 0; node < graph.NodesCount; ++node)
            {
                for (int adjancentNode = 0; adjancentNode < graph.NodesCount; ++adjancentNode)
                    data[node, adjancentNode] = (-1, double.MaxValue, double.MaxValue);
                data[node, node] = (node, 0, 0);

                (int, double)[] adjancentNodes = graph.Data[node];
                for (int incendence = 0; incendence < adjancentNodes.Length; ++incendence)
                {
                    (int adjancentNode, double adjancentWeight) = adjancentNodes[incendence];
                    data[node, adjancentNode] = (node, adjancentWeight, adjancentWeight);
                }
            }

            for (int k = 0; k < graph.NodesCount; ++k)
                for (int node = 0; node < graph.NodesCount; ++node)
                    for (int adjancentNode = 0; adjancentNode < graph.NodesCount; ++adjancentNode)
                        if (data[node, k].Item1 != -1 && data[k, adjancentNode].Item1 != -1 && data[node, adjancentNode].Item3 > data[node, k].Item3 + data[k, adjancentNode].Item3)
                            data[node, adjancentNode] = (data[k, adjancentNode].Item1, data[k, adjancentNode].Item2, data[node, k].Item3 + data[k, adjancentNode].Item3);

            for (int node = 0; node < graph.NodesCount; ++node)
                if (data[node, node].Item3 < 0)
                    throw new ArgumentException("negative loop");
        }
    }
}
