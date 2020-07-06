namespace lesson._16.cs
{
    class NodeStack<T>
    {
        public int size;
        public Node<T> root;

        public NodeStack()
        {
            size = 0;
            root = null;
        }

        public NodeStack(Node<T> root)
        {
            this.root = root;
            size = 0;
            for (Node<T> node = root; node != null; node = node.next)
                ++size;
        }

        public void Push(T value)
        {
            root = new Node<T>(value, root);
            ++size;
        }

        public T Pop()
        {
            Node<T> node = root;
            root = root.next;
            --size;
            return node.value;
        }
    }
}
