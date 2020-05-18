namespace lesson._04.cs
{
    class ListArray<T> : IArray<T>
    {
        Node<T> head;
        Node<T> tail;
        int size;

        public ListArray()
        {
            size = 0;
        }

        public int Size()
        {
            return size;
        }

        public void Add(T item)
        {
            Node<T> newNode = new Node<T>(item, null);
            if (size == 0)
                head = tail = newNode;
            else
            {
                tail.Next = newNode;
                tail = tail.Next;
            }
            ++size;
        }

        public void Add(T item, int index)
        {
            if (index == size)
                Add(item);
            else
            {
                Node<T> current;
                Node<T> prev;
                (current, prev) = FindNode(index);
                if (prev == null)
                    head = new Node<T>(item, head);
                else
                    prev.Next = new Node<T>(item, prev.Next);
                ++size;
            }
        }

        public T Get(int index)
        {
            Node<T> current;
            (current, _) = FindNode(index);
            return current.Item;
        }

        public void Set(T item, int index)
        {
            Node<T> current;
            (current, _) = FindNode(index);
            current.Item = item;
        }

        public T Remove(int index)
        {
            Node<T> current;
            Node<T> prev;
            (current, prev) = FindNode(index);

            if (prev == null)
                head = current.Next;
            else
                prev.Next = current.Next;

            if (current.Next == null)
                tail = prev;

            --size;

            return current.Item;
        }

        (Node<T>, Node<T>) FindNode(int index)
        {
            Node<T> current = head;
            Node<T> prev = null;
            while (index > 0 && current != null)
            {
                prev = current;
                current = current.Next;
                --index;
            }
            return (current, prev);
        }
    }
}
