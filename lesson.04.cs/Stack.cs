using System;
using System.Collections.Generic;
using System.Text;

namespace lesson._04.cs
{
    class Stack<T>
    {
        Node<T> head;

        public bool IsEmpty { get { return head == null; } }

        public void Push(T item)
        {
            head = new Node<T>(item, head);
        }

        public T Pop()
        {
            if (IsEmpty)
                throw new Exception("empty collection");

            T item = head.Item;

            head = head.Next;

            return item;
        }
    }
}
