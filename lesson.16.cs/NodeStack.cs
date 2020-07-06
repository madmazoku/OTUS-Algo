namespace lesson._16.cs
{
    class NodeStack<T>
    {
        public int size;
        public Node<T> top;

        public T Top {  get { return top.value; } set { top.value = value; } }

        public Node<T> Detach()
        {
            Node<T> root = top;
            top = null;
            size = 0;
            return root;
        }

        public NodeStack()
        {
            size = 0;
            top = null;
        }

        public NodeStack(Node<T> top)
        {
            this.top = top;
            size = 0;
            for (Node<T> node = top; node != null; node = node.next)
                ++size;
        }

        public void Push(T value)
        {
            top = new Node<T>(value, top);
            ++size;
        }

        public T Pop()
        {
            Node<T> node = top;
            top = top.next;
            --size;
            return node.value;
        }
    }
}
