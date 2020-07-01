using System;

namespace lesson._15.cs
{
    class Graph
    {
        public int[][] adjacenceArray;

        public Graph(int[][] adjacenceArray)
        {
            this.adjacenceArray = adjacenceArray;
        }

        public NodeList<int> DSF_recursive(int from, Func<NodeList<int>, bool> predicat)
        {
            if (from < 0 || from >= adjacenceArray.Length)
                throw new IndexOutOfRangeException();

            bool[] used = new bool[adjacenceArray.Length];
            NodeList<int> path = new NodeList<int>();

            used[from] = true;
            path.InsertLast(from);

            if (!DSF_recursive(used, path, predicat))
                path.RemoveLast();

            return path;
        }

        bool DSF_recursive(bool[] used, NodeList<int> path, Func<NodeList<int>, bool> predicat)
        {
            int[] adjacence = adjacenceArray[path.Tail.Value];
            foreach (int node in adjacence)
            {
                if (used[node])
                    continue;
                path.InsertLast(node);
                used[node] = true;
                if (predicat(path) || DSF_recursive(used, path, predicat))
                    return true;
                used[node] = false;
                path.RemoveLast();
            }
            return false;
        }

        public NodeList<int> DSF_iterative(int from, Func<NodeList<int>, bool> predicat)
        {
            if (from < 0 || from >= adjacenceArray.Length)
                throw new IndexOutOfRangeException();

            NodeList<int> path = new NodeList<int>();
            NodeList<int> incident = new NodeList<int>();
            bool[] used = new bool[adjacenceArray.Length];

            path.InsertLast(from);
            incident.InsertLast(0);

            do
            {
                if (predicat(path))
                    break;
            }
            while (DSF_next(used, path, incident));

            return path;
        }

        public bool DSF_next(bool[] used, NodeList<int> path, NodeList<int> incident)
        {
            while (incident.Size > 0)
            {
                bool isLoop = path.Size > 1 && path.Tail.Value == path.Head.Value; 
                bool isNoIncedentLeft = incident.Tail.Value == adjacenceArray[path.Tail.Value].Length;
                if (isLoop || isNoIncedentLeft)
                {
                    used[path.Tail.Value] = false;
                    path.RemoveLast();
                    incident.RemoveLast();
                }
                else
                {
                    int node = adjacenceArray[path.Tail.Value][incident.Tail.Value++];
                    bool isDegenerativeLoop = node == path.Head.Value && path.Size == 2;
                    if (!(used[node] || isDegenerativeLoop))
                    {
                        path.InsertLast(node);
                        incident.InsertLast(0);
                        used[node] = true;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
