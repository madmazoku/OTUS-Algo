using System.ComponentModel.Design.Serialization;
using System.Drawing;

namespace lesson._16.cs
{
    class NodeQueue<T>
    {
        public int size;
        public Node<T> first;
        public Node<T> last;

        public T First { get { return first.value; } set { first.value = value; } }
        public T Last { get { return last.value; } set { last.value = value; } }

        public Node<T> Detach()
        {
            Node<T> root = first;
            first = last = null;
            size = 0;
            return root;
        }

        public NodeQueue()
        {
            size = 0;
            last = first = null;
        }

        public NodeQueue(Node<T> first)
        {
            this.first = first;
            last = null;
            size = 0;
            for (Node<T> node = first; node != null; last = node, node = node.next)
                ++size;
        }

        public void Enque(T value)
        {
            Node<T> node = new Node<T>(value, null);
            if (last == null)
                first = last = node;
            else
                last = last.next = node;
            ++size;
        }

        public T Deque()
        {
            Node<T> node = first;
            first = first.next;
            if (first == null)
                last = null;
            --size;
            return node.value;
        }
    }
}
