namespace lesson._19.cs
{
    class UnionFind
    {
        int[] root;
        int[] size;

        public int Groups { get; private set; }

        public UnionFind(int nodeCount)
        {
            root = new int[nodeCount];
            size = new int[nodeCount];
            for (int index = 0; index < nodeCount; ++index)
            {
                root[index] = index;
                size[index] = 1;
            }
            Groups = nodeCount;
        }

        public int Find(int node)
        {
            while (root[node] != node)
                node = root[node];
            return node;
        }

        int Size(int node)
        {
            return size[Find(node)];
        }

        public bool HasOneRoot(int from, int to)
        {
            return Find(from) == Find(to);
        }

        public void Merge(int from, int to)
        {
            int toRoot = Find(to);
            int fromRoot = Find(from);
            if (toRoot == fromRoot)
                return;

            if (size[toRoot] < size[fromRoot])
            {
                root[toRoot] = fromRoot;
                size[toRoot] += size[fromRoot];
            }
            else
            {
                root[fromRoot] = toRoot;
                size[fromRoot] += size[toRoot];
            }

            --Groups;
        }
    }
}
