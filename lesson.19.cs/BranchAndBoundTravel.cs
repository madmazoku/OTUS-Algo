using System;
using System.CodeDom;
using System.Collections.Generic;

namespace lesson._19.cs
{
    class BranchAndBoundTravel
    {
        Node[] _nodes;
        List<Edge> _edges;

        public List<Edge> Edges { get { Build(); return _edges; } }

        public BranchAndBoundTravel(List<Node> nodes)
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

            double[,] adjancenceArray = new double[_nodes.Length, _nodes.Length];
            for (int from = 0; from < _nodes.Length; ++from)
            {
                for (int to = from + 1; to < _nodes.Length; ++to)
                    adjancenceArray[from, to] = adjancenceArray[to, from] = Node.Distance(_nodes[from], _nodes[to]);
                adjancenceArray[from, from] = double.MaxValue;
            }

            //adjancenceArray = new double[6, 6] {
            //    { 0,4,10,13,4,8 },
            //    { 2, 0, 9, 7, 6, 7 } ,
            //    { 8,5,0,5,5,9},
            //    { 5,8,5,0,7,10},
            //    { 6,4,4,9,0,4},
            //    { 5,1,4,8,3, 0}
            //};
            //for (int i = 0; i < _nodes.Length; ++i)
            //    adjancenceArray[i, i] = double.MaxValue;

            double[] rows = new double[_nodes.Length];
            double[] cols = new double[_nodes.Length];

            List<(int, int, double)> estimates = new List<(int, int, double)>();

            do
            {
                // Нахождение минимума по строкам
                Array.Fill(rows, double.MaxValue);
                for (int row = 0; row < _nodes.Length; ++row)
                    for (int col = 0; col < _nodes.Length; ++col)
                        if (adjancenceArray[row, col] != double.MaxValue && adjancenceArray[row, col] < rows[row])
                            rows[row] = adjancenceArray[row, col];

                // Редукция строк
                for (int row = 0; row < _nodes.Length; ++row)
                    for (int col = 0; col < _nodes.Length; ++col)
                        if (adjancenceArray[row, col] != double.MaxValue)
                            adjancenceArray[row, col] -= rows[row];

                // Нахождение минимума по столбцам.
                Array.Fill(cols, double.MaxValue);
                for (int col = 0; col < _nodes.Length; ++col)
                    for (int row = 0; row < _nodes.Length; ++row)
                        if (adjancenceArray[row, col] != double.MaxValue && adjancenceArray[row, col] < cols[col])
                            cols[col] = adjancenceArray[row, col];

                // Редукция столбцов
                for (int col = 0; col < _nodes.Length; ++col)
                    for (int row = 0; row < _nodes.Length; ++row)
                        if (adjancenceArray[row, col] != double.MaxValue)
                            adjancenceArray[row, col] -= cols[col];

                // Вычисление оценок нулевых клеток
                estimates.Clear();
                for (int row = 0; row < _nodes.Length; ++row)
                    for (int col = 0; col < _nodes.Length; ++col)
                        if (adjancenceArray[row, col] == 0)
                        {
                            double minRowValue = double.MaxValue;
                            double minColValue = double.MaxValue;
                            for (int i = 0; i < _nodes.Length; ++i)
                            {
                                if (i != row && adjancenceArray[i, col] != double.MaxValue && adjancenceArray[i, col] < minRowValue)
                                    minRowValue = adjancenceArray[i, col];
                                if (i != col && adjancenceArray[row, i] != double.MaxValue && adjancenceArray[row, i] < minColValue)
                                    minColValue = adjancenceArray[row, i];
                            }
                            estimates.Add((row, col, minRowValue + minColValue));
                        }

                // Редукция матрицы
                (int minRow, int minCol, double maxEstimate) = (-1, -1, double.MinValue);
                foreach ((int row, int col, double estimate) in estimates)
                    if (estimate > maxEstimate)
                        (minRow, minCol, maxEstimate) = (row, col, estimate);
                for (int i = 0; i < _nodes.Length; ++i)
                    adjancenceArray[minRow, i] = adjancenceArray[i, minCol] = double.MaxValue;
                adjancenceArray[minCol, minRow] = double.MaxValue;
                //for (int i = 0; i < _nodes.Length; ++i)
                //    adjancenceArray[i, minRow] = adjancenceArray[minCol, i] = double.MaxValue;
                //adjancenceArray[minRow, minCol] = double.MaxValue;

                _edges.Add(new Edge(minRow, minCol, Node.Distance(_nodes[minRow], _nodes[minCol])));

            } while (_edges.Count != _nodes.Length);
        }

        void ReduceRows(double[,] adjancenceArray)
        {
            double[] minRowDistance = new double[_nodes.Length];
            Array.Fill(minRowDistance, double.MaxValue);

            for (int row = 0; row < _nodes.Length; ++row)
                for (int col = 0; col < _nodes.Length; ++col)
                    if (adjancenceArray[row, col] != double.MaxValue && minRowDistance[row] > adjancenceArray[row, col])
                        minRowDistance[row] = adjancenceArray[row, col];

            for (int row = 0; row < _nodes.Length; ++row)
                for (int col = 0; col < _nodes.Length; ++col)
                    if (adjancenceArray[row, col] != double.MaxValue)
                        adjancenceArray[row, col] -= minRowDistance[row];

        }

        void ReduceCols(double[,] adjancenceArray)
        {
            double[] minColDistance = new double[_nodes.Length];
            Array.Fill(minColDistance, double.MaxValue);

            for (int row = 0; row < _nodes.Length; ++row)
                for (int col = 0; col < _nodes.Length; ++col)
                    if (adjancenceArray[row, col] != double.MaxValue && minColDistance[col] > adjancenceArray[row, col])
                        minColDistance[col] = adjancenceArray[row, col];

            for (int row = 0; row < _nodes.Length; ++row)
                for (int col = 0; col < _nodes.Length; ++col)
                    if (adjancenceArray[row, col] != double.MaxValue)
                        adjancenceArray[row, col] -= minColDistance[col];

        }

    }
}
