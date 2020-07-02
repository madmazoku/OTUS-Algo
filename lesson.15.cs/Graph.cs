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

        public NodeList<int> DFS(int from, Func<NodeList<int>, bool> predicat)
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
            while (DFS_next(used, path, incident));

            return path;
        }

        bool DFS_next(bool[] used, NodeList<int> path, NodeList<int> incident)
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

        public NodeList<int> BFS(int from, Func<NodeList<int>, bool> predicat)
        {
            if (from < 0 || from >= adjacenceArray.Length)
                throw new IndexOutOfRangeException();

            NodeList<NodeList<int>> queue = new NodeList<NodeList<int>>();
            bool[] used = new bool[adjacenceArray.Length];

            queue.InsertLast(new NodeList<int>());
            queue.Tail.Value.InsertLast(from);

            do
            {
                if (predicat(queue.Tail.Value))
                    break;
            }
            while (BFS_next(used, queue));

            return queue.Size > 0 ? queue.Tail.Value : new NodeList<int>();
        }

        bool BFS_next(bool[] used, NodeList<NodeList<int>> queue)
        {
            NodeList<int> path = queue.RemoveLast().Value;
            bool isLoop = path.Size > 1 && path.Tail.Value == path.Head.Value;
            if (isLoop)
                return queue.Size > 0;

            Array.Clear(used, 0, used.Length);
            foreach (int node in path.Values)
                used[node] = true;
            used[path.Head.Value] = false;

            foreach (int node in adjacenceArray[path.Tail.Value])
            {
                bool isDegenerativeLoop = node == path.Head.Value && path.Size == 2;
                if (!(used[node] || isDegenerativeLoop))
                {
                    queue.InsertFirst(path.Clone());
                    queue.Head.Value.InsertLast(node);
                }
            }

            return queue.Size > 0;
        }
    }
}
