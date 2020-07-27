using System;
using System.Collections.Generic;

namespace lesson._19.cs
{
    class BruteforceTravel
    {
        Node[] _nodes;
        List<Edge> _edges;

        public List<Edge> Edges { get { Build(); return _edges; } }

        double[,] _adjancenceArray;
        bool[] _visited;

        double _minPathDistance;
        int[] _minPath;

        double _pathDistance;
        int[] _path;

        public BruteforceTravel(List<Node> nodes)
        {
            this._nodes = nodes.ToArray();
            _edges = null;

            _adjancenceArray = null;
            _visited = null;

            _minPathDistance = double.MaxValue;
            _minPath = null;

            _pathDistance = double.MaxValue;
            _path = null;
        }

        public void Build()
        {
            if (_edges != null)
                return;

            _edges = new List<Edge>();
            if (_nodes.Length == 0)
                return;

            _adjancenceArray = new double[_nodes.Length, _nodes.Length];
            for (int from = 0; from < _nodes.Length - 1; ++from)
                for (int to = from + 1; to < _nodes.Length; ++to)
                    _adjancenceArray[from, to] = _adjancenceArray[to, from] = Node.Distance(_nodes[from], _nodes[to]);

            _visited = new bool[_nodes.Length];

            _minPathDistance = double.MaxValue;
            _minPath = new int[_nodes.Length];

            _pathDistance = 0;
            _path = new int[_nodes.Length];

            BuildPath(0, 0, 0);

            for (int level = 0; level < _nodes.Length - 1; ++level)
            {
                int from = _minPath[level];
                int to = _minPath[level + 1];
                _edges.Add(new Edge(from, to, _adjancenceArray[from, to]));
            }
            {
                int from = _minPath[_nodes.Length - 1];
                int to = _minPath[0];
                _edges.Add(new Edge(from, to, _adjancenceArray[from, to]));
            }
        }

        void BuildPath(int nodeIdx, double pathDistance, int level)
        {
            _path[level] = nodeIdx;
            _visited[nodeIdx] = true;

            if (level + 1 < _nodes.Length)
            {
                for (int idx = 0; idx < _nodes.Length; ++idx)
                    if (!_visited[idx])
                    {
                        double distance = _adjancenceArray[_path[level], idx];
                        if (_pathDistance + distance < _minPathDistance)
                            BuildPath(idx, pathDistance + distance, level + 1);
                    }
            }
            else
            {
                double distance = _adjancenceArray[_path[level], 0];
                if (pathDistance + distance < _minPathDistance)
                {
                    _minPathDistance = pathDistance + distance;
                    Array.Copy(_path, _minPath, _nodes.Length);
                }

            }

            _visited[nodeIdx] = false;
        }
    }
}
