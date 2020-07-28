using System;
using System.Collections.Generic;

namespace lesson._19.cs
{
    class BranchAndBoundTravel
    {
        GraphNode[] _nodes;
        List<GraphEdge> _edges;

        public List<GraphEdge> Edges { get { Build(); return _edges; } }

        public BranchAndBoundTravel(List<GraphNode> nodes)
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

            // 1) Построение матрицы с исходными данными.
            double[,] adjancenceArray = new double[_nodes.Length, _nodes.Length];
            for (int from = 0; from < _nodes.Length; ++from)
            {
                for (int to = from + 1; to < _nodes.Length; ++to)
                    adjancenceArray[from, to] = adjancenceArray[to, from] = GraphNode.Distance(_nodes[from], _nodes[to]);
                adjancenceArray[from, from] = double.MaxValue;
            }

            double[] minRowsDistance = new double[_nodes.Length];
            double[] minColsDistance = new double[_nodes.Length];

            do
            {
                // 2) Нахождение минимума по строкам.
                Array.Fill(minRowsDistance, double.MaxValue);
                for (int row = 0; row < _nodes.Length; ++row)
                    for (int col = 0; col < _nodes.Length; ++col)
                        if (adjancenceArray[row, col] != double.MaxValue && minRowsDistance[row] > adjancenceArray[row, col])
                            minRowsDistance[row] = adjancenceArray[row, col];

                // 3) Редукция строк.
                for (int row = 0; row < _nodes.Length; ++row)
                    for (int col = 0; col < _nodes.Length; ++col)
                        if (adjancenceArray[row, col] != double.MaxValue)
                            adjancenceArray[row, col] -= minRowsDistance[row];

                // 4) Нахождение минимума по столбцам
                Array.Fill(minColsDistance, double.MaxValue);
                for (int row = 0; row < _nodes.Length; ++row)
                    for (int col = 0; col < _nodes.Length; ++col)
                        if (adjancenceArray[row, col] != double.MaxValue && minColsDistance[col] > adjancenceArray[row, col])
                            minColsDistance[col] = adjancenceArray[row, col];

                // 5) Редукция столбцов.
                for (int row = 0; row < _nodes.Length; ++row)
                    for (int col = 0; col < _nodes.Length; ++col)
                        if (adjancenceArray[row, col] != double.MaxValue)
                            adjancenceArray[row, col] -= minColsDistance[col];

                // 6) Вычисление оценок нулевых клеток
                (int maxRow, int maxCol, double maxEstimate) = (-1, -1, double.MinValue);
                for (int row = 0; row < _nodes.Length; ++row)
                    for (int col = 0; col < _nodes.Length; ++col)
                        if (adjancenceArray[row, col] == 0)
                        {
                            double minRowDistance = double.MaxValue;
                            double minColDistance = double.MaxValue;
                            for (int i = 0; i < _nodes.Length; ++i)
                            {
                                if (i != col && adjancenceArray[row, i] != double.MaxValue && minRowDistance > adjancenceArray[row, i])
                                    minRowDistance = adjancenceArray[row, i];
                                if (i != row && adjancenceArray[i, col] != double.MaxValue && minColDistance > adjancenceArray[i, col])
                                    minColDistance = adjancenceArray[i, col];
                            }
                            double estimate = minRowDistance + minColDistance;
                            if (maxEstimate < estimate)
                                (maxRow, maxCol, maxEstimate) = (row, col, estimate);
                        }

                // 7) Редукция матрицы.
                for (int i = 0; i < _nodes.Length; ++i)
                    adjancenceArray[maxRow, i] = adjancenceArray[i, maxCol] = double.MaxValue;
                adjancenceArray[maxRow, maxCol] = adjancenceArray[maxCol, maxRow] = double.MaxValue;

                _edges.Add(new GraphEdge(maxRow, maxCol, GraphNode.Distance(_nodes[maxRow], _nodes[maxCol])));

                // 8) Если полный путь еще не найден, переходим к пункту 2, если найден к пункту 9.
            } while (_edges.Count != _nodes.Length);

            // 9) Вычисление итоговой длины пути и построение маршрута
        }
    }
}
