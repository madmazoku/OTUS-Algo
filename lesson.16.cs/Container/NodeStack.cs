using System;

namespace lesson._16.cs
{
    public class NodeStack<T>
    {
        public int size;
        public Node<T> top;

        public T Top { get { return top.value; } set { top.value = value; } }

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

        public NodeStack(T[] array)
        {
            top = null;
            for (int index = 0; index < array.Length; ++index)
                top = new Node<T>(array[index], top);
            size = array.Length;
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

        public bool Find(Func<T, bool> predicat)
        {
            for (Node<T> node = top; node != null; node = node.next)
                if (predicat(node.value))
                    return true;
            return false;
        }

        public ref T FindValue(Func<T, bool> predicat)
        {
            for (Node<T> node = top; node != null; node = node.next)
                if (predicat(node.value))
                    return ref node.value;
            throw new Exception();
        }

        public void InsertIf(T value, Func<T, bool> predicat)
        {
            Node<T> prev = null;
            Node<T> node = top;
            while (node != null && !predicat(node.value))
            {
                prev = node;
                node = node.next;
            }

            if (prev == null)
                top = new Node<T>(value, top);
            else
                prev.next = new Node<T>(value, prev.next);

            ++size;
        }

    }
}
