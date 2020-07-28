using lesson._16.cs;
using System;
using System.Collections.Generic;

// Поиск гамильтонова цикла в большом графе
// https://habr.com/ru/post/160077/

namespace lesson._19.cs
{
    class SolverNode : IComparable<SolverNode>
    {
        SolverNode _parent;
        SolverNode _left;
        SolverNode _right;

        GraphNode[] _nodes;
        List<GraphEdge> _edges;

        double _lowerBound;
        double _distance;

        double[,] _adjancenceArray;

        double[] _minRowsDistance;
        double[] _minColsDistance;

        public int CompareTo(SolverNode other)
        {
            return _lowerBound.CompareTo(other._lowerBound);
        }

        public SolverNode(GraphNode[] nodes)
        {
            _parent = null;
            _left = null;
            _right = null;

            _nodes = nodes;
            _adjancenceArray = new double[_nodes.Length, _nodes.Length];
            _edges = new List<GraphEdge>();
            _minRowsDistance = new double[_nodes.Length];
            _minColsDistance = new double[_nodes.Length];

            _lowerBound = 0;
            _distance = 0;

            for (int from = 0; from < _nodes.Length; ++from)
            {
                for (int to = from + 1; to < _nodes.Length; ++to)
                    _adjancenceArray[from, to] = _adjancenceArray[to, from] = GraphNode.Distance(_nodes[from], _nodes[to]);
                _adjancenceArray[from, from] = double.MaxValue;
            }

            Reduce();
        }

        public SolverNode(SolverNode solverNode)
        {
            _parent = solverNode;
            _left = null;
            _right = null;

            _nodes = solverNode._nodes;
            _lowerBound = solverNode._lowerBound;
            _distance = solverNode._distance;

            _adjancenceArray = new double[_nodes.Length, _nodes.Length];
            _edges = new List<GraphEdge>();
            _minRowsDistance = new double[_nodes.Length];
            _minColsDistance = new double[_nodes.Length];

            _adjancenceArray = solverNode._adjancenceArray.Clone() as double[,];

            foreach (GraphEdge edge in solverNode._edges)
                _edges.Add(edge);
        }

        public bool IsComplete { get { return _edges.Count == _nodes.Length; } }
        public double Distance { get { return IsComplete ? _distance : double.MaxValue; } }
        public List<GraphEdge> Edges { get { return _edges; } }

        public void Reduce()
        {
            Array.Fill(_minRowsDistance, double.MaxValue);
            for (int row = 0; row < _nodes.Length; ++row)
                for (int col = 0; col < _nodes.Length; ++col)
                    if (_adjancenceArray[row, col] != double.MaxValue && _minRowsDistance[row] > _adjancenceArray[row, col])
                        _minRowsDistance[row] = _adjancenceArray[row, col];

            for (int row = 0; row < _nodes.Length; ++row)
                for (int col = 0; col < _nodes.Length; ++col)
                    if (_adjancenceArray[row, col] != double.MaxValue)
                        _adjancenceArray[row, col] -= _minRowsDistance[row];

            Array.Fill(_minColsDistance, double.MaxValue);
            for (int row = 0; row < _nodes.Length; ++row)
                for (int col = 0; col < _nodes.Length; ++col)
                    if (_adjancenceArray[row, col] != double.MaxValue && _minColsDistance[col] > _adjancenceArray[row, col])
                        _minColsDistance[col] = _adjancenceArray[row, col];

            for (int row = 0; row < _nodes.Length; ++row)
                for (int col = 0; col < _nodes.Length; ++col)
                    if (_adjancenceArray[row, col] != double.MaxValue)
                        _adjancenceArray[row, col] -= _minColsDistance[col];

            for (int i = 0; i < _nodes.Length; ++i)
            {
                if (_minRowsDistance[i] != double.MaxValue)
                    _lowerBound += _minRowsDistance[i];
                if (_minColsDistance[i] != double.MaxValue)
                    _lowerBound += _minColsDistance[i];
            }
        }

        public (SolverNode, SolverNode) Split()
        {
            (int maxRow, int maxCol, double maxEstimate) = (-1, -1, double.MinValue);
            for (int row = 0; row < _nodes.Length; ++row)
                for (int col = 0; col < _nodes.Length; ++col)
                    if (_adjancenceArray[row, col] == 0)
                    {
                        double minRowDistance = double.MaxValue;
                        double minColDistance = double.MaxValue;
                        for (int i = 0; i < _nodes.Length; ++i)
                        {
                            if (i != col && _adjancenceArray[row, i] != double.MaxValue && minRowDistance > _adjancenceArray[row, i])
                                minRowDistance = _adjancenceArray[row, i];
                            if (i != row && _adjancenceArray[i, col] != double.MaxValue && minColDistance > _adjancenceArray[i, col])
                                minColDistance = _adjancenceArray[i, col];
                        }
                        double estimate = 0;
                        if (minRowDistance != double.MaxValue)
                            estimate += minRowDistance;
                        if (minColDistance != double.MaxValue)
                            estimate += minColDistance;
                        if (maxEstimate < estimate)
                            (maxRow, maxCol, maxEstimate) = (row, col, estimate);
                    }

            if (maxEstimate != double.MinValue)
            {
                _left = new SolverNode(this);
                _left.IncludeEdge(maxRow, maxCol, maxEstimate);

                _right = new SolverNode(this);
                _right.ExcludeEdge(maxRow, maxCol, maxEstimate);
            }

            return (_left, _right);
        }

        void IncludeEdge(int row, int col, double estimate)
        {
            double distance = GraphNode.Distance(_nodes[row], _nodes[col]);
            _distance += distance;
            _edges.Add(new GraphEdge(row, col, distance));

            for (int i = 0; i < _nodes.Length; ++i)
                _adjancenceArray[row, i] = _adjancenceArray[i, col] = double.MaxValue;
            _adjancenceArray[col, row] = double.MaxValue;

            Reduce();
        }

        void ExcludeEdge(int row, int col, double estimate)
        {
            _adjancenceArray[row, col] = double.MaxValue;
            _lowerBound += estimate;
        }

    };

    class BranchAndBound2Travel
    {
        GraphNode[] _nodes;
        List<GraphEdge> _edges;

        public List<GraphEdge> Edges { get { Build(); return _edges; } }

        public BranchAndBound2Travel(List<GraphNode> nodes)
        {
            this._nodes = nodes.ToArray();
            _edges = null;
        }

        public void Build()
        {
            if (_edges != null)
                return;

            _edges = new List<GraphEdge>();
            if (_nodes.Length == 0)
                return;

            SolverNode bestSolverNode = null;
            SolverNode root = new SolverNode(_nodes);
            NodeStack<SolverNode> solverNodes = new NodeStack<SolverNode>();
            solverNodes.Push(root);

            while (solverNodes.size > 0)
            {
                SolverNode currentSolverNode = solverNodes.Pop();
                if (currentSolverNode.IsComplete)
                {
                    if (bestSolverNode == null || bestSolverNode.CompareTo(currentSolverNode) > 0)
                        bestSolverNode = currentSolverNode;
                }
                else
                {
                    (SolverNode withEdge, SolverNode withoutEdge) = currentSolverNode.Split();
                    if (withEdge != null)
                        solverNodes.InsertIf(withEdge, (x) => { return withEdge.CompareTo(x) <= 0; });
                    if (withoutEdge != null)
                        solverNodes.InsertIf(withoutEdge, (x) => { return withoutEdge.CompareTo(x) <= 0; });
                }
            }

            _edges = bestSolverNode.Edges;
        }
    }
}
