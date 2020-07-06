namespace lesson._16.cs
{
    class NodeQueue<T>
    {
        public int size;
        public Node<T> root;
        public Node<T> last;

        public NodeQueue()
        {
            size = 0;
            last = root = null;
        }

        public NodeQueue(Node<T> root)
        {
            this.root = root;
            last = null;
            size = 0;
            for (Node<T> node = root; node != null; last = node, node = node.next)
                ++size;
        }

        public void Enque(T value)
        {
            Node<T> node = new Node<T>(value, null);
            if (last == null)
                root = last = node;
            else
                last = last.next = node;
            ++size;
        }

        public T Deque()
        {
            Node<T> node = root;
            root = root.next;
            if (root == null)
                last = null;
            --size;
            return node.value;
        }
    }
}
