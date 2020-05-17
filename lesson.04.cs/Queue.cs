using System;
using System.Collections.Generic;
using System.Text;

namespace lesson._04.cs
{
    class Queue<T>
    {
        Node<T> head;
        Node<T> tail;

        public bool IsEmpty { get { return head == null; } }

        public void enque(T item)
        {
            Node<T> node = new Node<T>(item, null);
            if (IsEmpty)
                head = tail = node;
            else 
                tail = tail.Next = node;
        }

        public T deque()
        {
            if (IsEmpty)
                throw new Exception("empty collection");

            T item = head.Item;
            
            head = head.Next;

            if (IsEmpty)
                tail = null;

            return item;
        }
    }
}
