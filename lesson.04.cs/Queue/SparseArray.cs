namespace lesson._04.cs
{
    class SparseArray<T> : IArray<T>
    {
        class SparseItem
        {
            int index;
            T item;

            public SparseItem(int index, T item)
            {
                this.index = index;
                this.item = item;
            }

            public int Index { get { return index; } set { index = value; } }
            public T Item { get { return item; } set { item = value; } }
        }
        Queue<SparseItem> queue;

        public SparseArray()
        {
            queue = new Queue<SparseItem>();
        }

        public int Size()
        {
            return queue.Tail != null ? queue.Tail.Item.Index + 1 : 0;
        }

        public void Add(T item)
        {
            queue.enque(new SparseItem(Size(), item));
        }

        public void Add(T item, int index)
        {
            Node<SparseItem> prev = null;
            bool found = false;
            foreach (Node<SparseItem> current in queue)
            {
                if (!found)
                {
                    int cmp = index.CompareTo(current.Item.Index);
                    if (cmp <= 0)
                    {
                        queue.enqueAfter(prev, new SparseItem(index, item));
                        found = true;
                    }
                }
                if (found)
                    ++current.Item.Index;
                prev = current;
            }
            if (!found)
                queue.enque(new SparseItem(index, item));
        }

        public T Get(int index)
        {
            foreach (Node<SparseItem> current in queue)
            {
                int cmp = index.CompareTo(current.Item.Index);
                if (cmp == -1)
                    break;
                else if (cmp == 0)
                    return current.Item.Item;
            }
            return default(T);
        }

        public void Set(T item, int index)
        {
            Node<SparseItem> prev = null;
            foreach (Node<SparseItem> current in queue)
            {
                int cmp = index.CompareTo(current.Item.Index);
                if (cmp == -1)
                    break;
                else if (cmp == 0)
                {
                    current.Item.Item = item;
                    return;
                }
                prev = current;
            }
            queue.enqueAfter(prev, new SparseItem(index, item));
        }
        public T Remove(int index)
        {
            Node<SparseItem> prev = null;
            bool found = false;
            T item = default(T);
            foreach (Node<SparseItem> current in queue)
            {
                if (!found)
                {
                    int cmp = index.CompareTo(current.Item.Index);
                    if (cmp == -1)
                        found = true;
                    else if (cmp == 0)
                    {
                        found = true;
                        item = queue.dequeNext(prev).Item;
                        continue;
                    }
                }
                if (found)
                    --current.Item.Index;
                prev = current;
            }
            return item;
        }

    }
}
