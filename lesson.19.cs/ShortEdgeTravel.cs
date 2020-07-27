using System;
using System.Collections.Generic;

namespace lesson._19.cs
{
    class ShortEdgeTravel
    {
        Node[] _nodes;
        List<Edge> _edges;

        public List<Edge> Edges { get { Build(); return _edges; } }

        public ShortEdgeTravel(List<Node> nodes)
        {
            this._nodes = nodes.ToArray();
            _edges = null;
        }

        public void Build()
        {
            if (_edges != null)
                return;

            _edges = new List<Edge>();
            if (_nodes.Length == 0)
                return;

            Edge[] edges = new Edge[(_nodes.Length * (_nodes.Length - 1)) >> 1];
            int cnt = 0;
            for (int from = 0; from < _nodes.Length - 1; ++from)
                for (int to = from + 1; to < _nodes.Length; ++to)
                    edges[cnt++] = new Edge(from, to, Node.Distance(_nodes[from], _nodes[to]));

            Array.Sort(edges, new EdgeComparer());

            UnionFind uf = new UnionFind(_nodes.Length);

            int[] rank = new int[_nodes.Length];

            for (int idx = 0; idx < edges.Length; ++idx)
            {
                Edge edge = edges[idx];
                if (uf.Groups > 1 && uf.HasOneRoot(edge.from, edge.to))
                    continue;
                if (rank[edge.from] > 1 || rank[edge.to] > 1)
                    continue;

                _edges.Add(edge);
                uf.Merge(edge.from, edge.to);
                ++rank[edge.from];
                ++rank[edge.to];

                if (_edges.Count == _nodes.Length)
                    break;
            }
        }

    }
}
