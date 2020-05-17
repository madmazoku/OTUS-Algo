using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace lesson._04.cs
{
    class Node<T>
    {
        private T item;
        private Node<T> next;

        public Node(T item, Node<T> next)
        {
            this.item = item;
            this.next = next;
        }

        public T Item { get { return item; } set { item = value; } }
        public Node<T> Next { get { return next; } set { next = value; }  }
    }
}
