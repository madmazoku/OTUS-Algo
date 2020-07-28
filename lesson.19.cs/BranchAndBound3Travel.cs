using System;
using System.Collections.Generic;

namespace lesson._19.cs
{

    class CostMatrix
    {
        int _size;
        double[,] _costs;

        double[,] _reduced;
        double _lowerBound;

        double LowerBound { get { return _lowerBound; } }

        public CostMatrix(int size, double[,] costs)
        {
            _size = size;
            _costs = costs;

            Array.Copy(costs, _costs, costs.Length);

            _reduced = new double[_size, _size];
            _lowerBound = 0;

            Reduce();
        }

        public CostMatrix(int size, double[,] costs, double[,] reduced, double lowerBound)
        {
            _size = size;
            _costs = costs;

            _reduced = reduced.Clone() as double[,];
            _lowerBound = lowerBound;
        }

        double Reduce()
        {
            double[] minRowsCost = new double[_size];
            double[] minColsCost = new double[_size];

            Array.Fill(minRowsCost, double.MaxValue);
            for (int row = 0; row < _size; ++row)
                for (int col = 0; col < _size; ++col)
                    if (_reduced[row, col] != double.MaxValue && minRowsCost[row] > _reduced[row, col])
                        minRowsCost[row] = _reduced[row, col];

            for (int row = 0; row < _size; ++row)
                for (int col = 0; col < _size; ++col)
                    if (_reduced[row, col] != double.MaxValue)
                        _reduced[row, col] -= minRowsCost[row];

            Array.Fill(minColsCost, double.MaxValue);
            for (int row = 0; row < _size; ++row)
                for (int col = 0; col < _size; ++col)
                    if (_reduced[row, col] != double.MaxValue && minColsCost[col] > _reduced[row, col])
                        minColsCost[col] = _reduced[row, col];

            for (int row = 0; row < _size; ++row)
                for (int col = 0; col < _size; ++col)
                    if (_reduced[row, col] != double.MaxValue)
                        _reduced[row, col] -= minColsCost[col];

            for (int i = 0; i < _size; ++i)
            {
                if (minRowsCost[i] != double.MaxValue)
                    _lowerBound += minRowsCost[i];
                if (minColsCost[i] != double.MaxValue)
                    _lowerBound += minColsCost[i];
            }

            return _lowerBound;
        }

        (int, int, double) FindCell()
        {
            (int maxRow, int maxCol, double maxEstimate) = (-1, -1, double.MinValue);
            for (int row = 0; row < _size; ++row)
                for (int col = 0; col < _size; ++col)
                    if (_reduced[row, col] == 0)
                    {
                        double minRowDistance = double.MaxValue;
                        double minColDistance = double.MaxValue;
                        for (int i = 0; i < _size; ++i)
                        {
                            if (i != col && _reduced[row, i] != double.MaxValue && minRowDistance > _reduced[row, i])
                                minRowDistance = _reduced[row, i];
                            if (i != row && _reduced[i, col] != double.MaxValue && minColDistance > _reduced[i, col])
                                minColDistance = _reduced[i, col];
                        }
                        double estimate = 0;
                        if (minRowDistance != double.MaxValue)
                            estimate += minRowDistance;
                        if (minColDistance != double.MaxValue)
                            estimate += minColDistance;
                        if (maxEstimate < estimate)
                            (maxRow, maxCol, maxEstimate) = (row, col, estimate);
                    }

            return (maxRow, maxCol, maxEstimate);
        }
    }

    class BranchAndBound3Travel
    {
        GraphNode[] _nodes;
        List<GraphEdge> _edges;

        public List<GraphEdge> Edges { get { Build(); return _edges; } }

        public BranchAndBound3Travel(List<GraphNode> nodes)
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

            double[,] _reduced = new double[_nodes.Length, _nodes.Length];
            for (int row = 0; row < _nodes.Length; ++row)
            {
                for (int col = row + 1; col < _nodes.Length; ++col)
                    _reduced[row, col] = _reduced[col, row] = GraphNode.Distance(_nodes[row], _nodes[col]);
                _reduced[row, row] = double.MaxValue;
            }

            CostMatrix matrix = new CostMatrix(_nodes.Length);
            matrix.SetCosts(_reduced);
        }
    }
}
