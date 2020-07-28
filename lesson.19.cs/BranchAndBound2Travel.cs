using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;

// Поиск гамильтонова цикла в большом графе
// https://habr.com/ru/post/160077/

namespace lesson._19.cs
{
    class SolverNode : IComparable<SolverNode>
    {
        Node[] _nodes;

        double _lowerBound;
        double[,] _adjancenceArray;
        bool[,] _infinityArray;

        int IComparable<SolverNode>.CompareTo(SolverNode other)
        {
            return _lowerBound.CompareTo(other._lowerBound);
        }

        public SolverNode(Node[] nodes)
        {
            _nodes = nodes;
            _adjancenceArray = new double[_nodes.Length, _nodes.Length];
            _infinityArray = new bool[_nodes.Length, _nodes.Length];
            _lowerBound = double.MaxValue;

            for (int from = 0; from < _nodes.Length; ++from)
            {
                for (int to = from + 1; to < _nodes.Length; ++to)
                    _adjancenceArray[from, to] = _adjancenceArray[to, from] = Node.Distance(_nodes[from], _nodes[to]);
                _adjancenceArray[from, from] = 0;
                _infinityArray[from, from] = true;
            }
        }

        public SolverNode(SolverNode solverNode)
        {
            _nodes = solverNode._nodes;
            _lowerBound = solverNode._lowerBound;

            _adjancenceArray = new double[_nodes.Length, _nodes.Length];
            _infinityArray = new bool[_nodes.Length, _nodes.Length];

            solverNode._adjancenceArray.CopyTo(_adjancenceArray, 0);
            solverNode._infinityArray.CopyTo(_infinityArray, 0);
        }
    };

    class BranchAndBound2Travel
    {
        Node[] _nodes;
        List<Edge> _edges;

        public List<Edge> Edges { get { Build(); return _edges; } }

        public BranchAndBound2Travel(List<Node> nodes)
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

            double distance = 0;
            do
            {
                double[] minRowDistance = ReduceRows(adjancenceArray);
                double[] minColDistance = ReduceCols(adjancenceArray);
                double lowerBound = GetLowerBound(minRowDistance, minColDistance);
                (int row, int col, double penalty) = FindEdge(adjancenceArray);

                double distanceWithoutEdge = distance + penalty;

                double[,] adjancenceArrayWithEdge = new double[_nodes.Length, _nodes.Length];
                Array.Copy(adjancenceArray, adjancenceArrayWithEdge, adjancenceArray.Length);

                // Редукция матрицы
                for (int i = 0; i < _nodes.Length; ++i)
                    adjancenceArrayWithEdge[row, i] = adjancenceArrayWithEdge[i, col] = double.MaxValue;
                adjancenceArrayWithEdge[row, col] = adjancenceArrayWithEdge[col, row] = double.MaxValue;
                double[] minRowDistanceWithEdge = ReduceRows(adjancenceArrayWithEdge);
                double[] minColDistanceWithEdge = ReduceCols(adjancenceArrayWithEdge);
                double penaltyWithEdge = minRowDistanceWithEdge[row] + minColDistanceWithEdge[col];

                double distanceWithEdge = distance + penaltyWithEdge;

                //for (int i = 0; i < _nodes.Length; ++i)
                //    adjancenceArray[row, i] = adjancenceArray[i, col] = double.MaxValue;
                //adjancenceArray[row, col] = adjancenceArray[col, row] = double.MaxValue;

                _edges.Add(new Edge(row, col, Node.Distance(_nodes[row], _nodes[col])));

            } while (_edges.Count != _nodes.Length);
        }

        double[] ReduceRows(double[,] adjancenceArray)
        {
            double[] minRowDistance = new double[_nodes.Length];
            Array.Fill(minRowDistance, double.MaxValue);

            // 1.1. Вычисляем наименьший элемент в каждой строке (константа приведения для строки)
            for (int row = 0; row < _nodes.Length; ++row)
                for (int col = 0; col < _nodes.Length; ++col)
                    if (adjancenceArray[row, col] != double.MaxValue && minRowDistance[row] > adjancenceArray[row, col])
                        minRowDistance[row] = adjancenceArray[row, col];

            // 1.2. Переходим к новой матрице затрат, вычитая из каждой строки ее константу приведения
            for (int row = 0; row < _nodes.Length; ++row)
                for (int col = 0; col < _nodes.Length; ++col)
                    if (adjancenceArray[row, col] != double.MaxValue)
                        adjancenceArray[row, col] -= minRowDistance[row];

            return minRowDistance;
        }

        double[] ReduceCols(double[,] adjancenceArray)
        {
            double[] minColDistance = new double[_nodes.Length];
            Array.Fill(minColDistance, double.MaxValue);

            // 1.3.Вычисляем наименьший элемент в каждом столбце(константа приведения для столбца)
            for (int row = 0; row < _nodes.Length; ++row)
                for (int col = 0; col < _nodes.Length; ++col)
                    if (adjancenceArray[row, col] != double.MaxValue && minColDistance[col] > adjancenceArray[row, col])
                        minColDistance[col] = adjancenceArray[row, col];

            // 1.4. Переходим к новой матрице затрат, вычитая из каждого столбца его константу приведения.
            for (int row = 0; row < _nodes.Length; ++row)
                for (int col = 0; col < _nodes.Length; ++col)
                    if (adjancenceArray[row, col] != double.MaxValue)
                        adjancenceArray[row, col] -= minColDistance[col];

            return minColDistance;
        }

        double GetLowerBound(double[] minRowDistance, double[] minColDistance)
        {
            // 1.5. Вычисляем границу на данном этапе как сумму констант приведения для столбцов и строк 
            //    (данная граница будет являться стоимостью, меньше которой невозможно построить искомый маршрут)
            double lowerBound = 0;
            for (int i = 0; i < _nodes.Length; ++i)
                lowerBound += minRowDistance[i] + minColDistance[i];
            return lowerBound;
        }

        (int, int, double) FindEdge(double[,] adjancenceArray)
        {
            (int maxRow, int maxCol, double maxPenalty) = (-1, -1, double.MinValue);

            // 2.1. Вычисление штрафа за неиспользование для каждого нулевого элемента приведенной матрицы затрат.
            //      Штраф за неиспользование элемента с индексом(h, k) в матрице, означает, 
            //      что это ребро не включается в наш маршрут, а значит минимальная стоимость «неиспользования» 
            //      этого ребра равна сумме минимальных элементов в строке h и столбце k.
            //
            // 2.1.а. Ищем все нулевые элементы в приведенной матрице
            // 2.1.б. Для каждого из них считаем его штраф за неиспользование.
            // 2.1.в. Выбираем элемент, которому соответствует максимальный штраф(любой, если их несколько)

            for (int row = 0; row < _nodes.Length; ++row)
                for (int col = 0; col < _nodes.Length; ++col)
                    if(adjancenceArray[row, col] == 0)
                    {
                        double minRowDistance = double.MaxValue;
                        double minColDistance = double.MaxValue;
                        for(int i = 0; i < _nodes.Length; ++i)
                        {
                            if (i != col && adjancenceArray[row, i] != double.MaxValue && minRowDistance > adjancenceArray[row, i])
                                minRowDistance = adjancenceArray[row, i];
                            if (i != row && adjancenceArray[i, col] != double.MaxValue && minColDistance > adjancenceArray[i, col])
                                minColDistance = adjancenceArray[i, col];
                        }
                        double penalty = minRowDistance + minColDistance;
                        if (penalty > maxPenalty)
                            (maxRow, maxCol, maxPenalty) = (row, col, penalty);
                    }

            return (maxRow, maxCol, maxPenalty);
        }


    }
}
