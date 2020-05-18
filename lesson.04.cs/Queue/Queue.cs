using System;
using System.Collections;

namespace lesson._04.cs
{
    class Queue<T> : IEnumerable
    {
        Node<T> head;
        Node<T> tail;
        int size;

        public bool IsEmpty { get { return head == null; } }
        public int Size { get { return size; } }
        public Node<T> Head { get { return head; } }
        public Node<T> Tail { get { return tail; } }

        public Queue()
        {
            size = 0;
        }

        public void enque(T item)
        {
            ++size;
            Node<T> newNode = new Node<T>(item, null);
            if (IsEmpty)
                head = tail = newNode;
            else
                tail = tail.Next = newNode;
        }

        public void enqueAfter(Node<T> node, T item)
        {
            ++size;
            Node<T> newNode = new Node<T>(item, null);
            if (node == null)
            {
                newNode.Next = head;
                head = newNode;
            }
            else
            {
                newNode.Next = node.Next;
                node.Next = newNode;
            }
            if (newNode.Next == null)
                tail = newNode;
        }


        public T deque()
        {
            if (IsEmpty)
                throw new Exception("empty collection");

            --size;

            T item = head.Item;

            head = head.Next;

            if (IsEmpty)
                tail = null;

            return item;
        }

        public T dequeNext(Node<T> node)
        {
            if (IsEmpty)
                throw new Exception("empty collection");

            --size;

            T item;
            if (node == null)
            {
                item = head.Item;
                head = head.Next;
                if (IsEmpty)
                    tail = null;
            }
            else
            {
                if (node.Next == null)
                    throw new IndexOutOfRangeException();

                item = node.Next.Item;
                node.Next = node.Next.Next;
            }

            return item;
        }

        public T[] Array
        {
            get
            {
                T[] array = new T[size];
                int index = 0;
                foreach (Node<T> node in this)
                {
                    array[index] = node.Item;
                    ++index;
                }
                return array;
            }
        }

        public IEnumerator GetEnumerator()
        {
            Node<T> current = head;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }
    }
}
