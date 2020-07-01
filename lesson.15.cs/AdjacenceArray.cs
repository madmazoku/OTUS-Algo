using System;
using System.Linq;

namespace lesson._15.cs
{
    class AdjacenceArray
    {
        NodeList<(int a, int b, bool directed)> edges;

        public int Nodes { get; set; }

        public AdjacenceArray(int nodes)
        {
            Nodes = nodes;
            edges = new NodeList<(int, int, bool)>();
        }

        public void AddEdge(int from, int to, bool directed)
        {
            if (from < 0 || from >= Nodes || to < 0 || to >= Nodes)
                throw new IndexOutOfRangeException();

            edges.Enque((from, to, directed));
        }

        public void AddEdges(int from, int[] toArray, bool directed)
        {
            foreach (int to in toArray)
                AddEdge(from, to, directed);
        }

        public int[][] Build()
        {
            NodeList<int>[] adjacenceList = new NodeList<int>[Nodes];
            foreach ((int from, int to, bool directed) in edges.Values)
            {
                if (adjacenceList[from] == null)
                    adjacenceList[from] = new NodeList<int>();
                adjacenceList[from].InsertIf(to, (newValue, listValue) => { return newValue > listValue; });
                if (!directed && to != from)
                {
                    if (adjacenceList[to] == null)
                        adjacenceList[to] = new NodeList<int>();
                    adjacenceList[to].InsertIf(from, (newValue, listValue) => { return newValue > listValue; });
                }
            }

            int[][] adjanceArray = new int[Nodes][];
            for (int index = 0; index < Nodes; ++index)
                if (adjacenceList[index] != null && adjacenceList[index].Size > 0)
                    adjanceArray[index] = adjacenceList[index].Values.ToArray();
                else
                    adjanceArray[index] = new int[0];

            return adjanceArray;
        }


    }
}
