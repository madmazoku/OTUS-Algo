using System;

namespace lesson._16.cs
{
    public class NodeQueue<T>
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

        public NodeQueue(T[] array)
        {
            first = last = null;
            if (array.Length > 0)
            {
                first = last = new Node<T>(array[0], null);
                for (int index = 0; index < array.Length; ++index)
                    last = last.next = new Node<T>(array[index], null);
            }
            size = array.Length;
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

        public bool Find(Func<T, bool> predicat)
        {
            for (Node<T> node = first; node != null; node = node.next)
                if (predicat(node.value))
                    return true;
            return false;
        }

        public ref T FindValue(Func<T, bool> predicat)
        {
            for (Node<T> node = first; node != null; node = node.next)
                if (predicat(node.value))
                    return ref node.value;
            throw new Exception();
        }

        public void InsertIf(T value, Func<T, bool> predicat)
        {
            Node<T> prev = null;
            Node<T> node = first;
            while (node != null && !predicat(node.value))
            {
                prev = node;
                node = node.next;
            }

            Node<T> newNode;
            if (prev == null)
                newNode = first = new Node<T>(value, first);
            else
                newNode = prev.next = new Node<T>(value, prev.next);

            if (newNode.next == null)
                last = newNode;

            ++size;
        }
    }
}
