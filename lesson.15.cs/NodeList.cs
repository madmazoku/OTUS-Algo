using System;
using System.Collections;
using System.Collections.Generic;

namespace lesson._15.cs
{
    class NodeList<T>
    {
        public class Node
        {
            public T Value { get; set; }
            public Node Prev { get; set; }
            public Node Next { get; set; }

            public Node(T value, Node prev, Node next)
            {
                Value = value;
                Prev = prev;
                Next = next;
            }
        }

        public Node Head { get; private set; }
        public Node Tail { get; private set; }
        public int Size { get; private set; }

        public Node Enque(T value) { return InsertLast(value); }
        public Node Deque() { return RemoveFirst(); }

        public Node Push(T value) { return InsertFirst(value); }
        public Node Pop() { return RemoveFirst(); }

        public Node InsertLast(T value)
        {
            Node node = new Node(value, Tail, null);
            if (Head == null)
                Head = Tail = node;
            else
                Tail = Tail.Next = node;
            ++Size;
            return node;
        }

        public Node RemoveLast()
        {
            Node node = Tail;
            if (Tail.Prev == null)
                Head = Tail = null;
            else
                (Tail = Tail.Prev).Next = null;
            --Size;
            return node;
        }

        public Node InsertFirst(T value)
        {
            Node node = new Node(value, null, Head);
            if (Tail == null)
                Tail = Head = node;
            else
                Head = Head.Prev = node;
            ++Size;
            return node;
        }

        public Node RemoveFirst()
        {
            Node node = Head;
            if (Head.Next == null)
                Head = Tail = null;
            else
                (Head = Head.Next).Prev = null;
            --Size;
            return node;
        }

        // utility
        public Node InsertIf(T value, Func<T, T, bool> predicat)
        {
            Node prev = null;
            Node next = Head;
            while (next != null && predicat(value, next.Value))
            {
                prev = next;
                next = next.Next;
            }
            Node node = new Node(value, prev, next);
            if (prev == null)
                Head = node;
            else
                prev.Next = node;
            if (next == null)
                Tail = node;
            else
                next.Prev = node;
            ++Size;
            return node;
        }

        public void RemoveNode(Node node)
        {
            if (node.Prev == null)
                Head = node.Next;
            else
                node.Prev.Next = node.Next;
            if (node.Next == null)
                Tail = node.Prev;
            else
                node.Next.Prev = node.Prev;
            --Size;
        }

        public Node Find(T value, Func<T, T, bool> predicat)
        {
            for (Node node = Head; node != null; node = node.Next)
                if (predicat(value, node.Value))
                    return node;
            return null;
        }

        public void Reverse()
        {
            Node node = Head;
            while (node != null)
            {
                (node.Prev, node.Next) = (node.Next, node.Prev);
                node = node.Prev;
            }
            (Head, Tail) = (Tail, Head);
        }

        // Enumerations
        public NodeEnumerable Nodes { get { return new NodeEnumerable(this); } }
        public ValueEnumerable Values { get { return new ValueEnumerable(this); } }

        public class NodeEnumerable : IEnumerable<Node>
        {
            NodeList<T> owner;

            public NodeEnumerable(NodeList<T> owner) { this.owner = owner; }

            public IEnumerator<Node> GetEnumerator() { return new NodeEnumerator(owner); }
            private IEnumerator GetEnumerator1() { return this.GetEnumerator(); }
            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator1(); }

        }

        public class NodeEnumerator : IEnumerator<Node>
        {
            NodeList<T> owner;

            public NodeEnumerator(NodeList<T> owner) { this.owner = owner; Current = null; }

            public Node Current { get; private set; }
            private object Current1 { get { return this.Current; } }
            object IEnumerator.Current { get { return this.Current1; } }

            public bool MoveNext()
            {
                if (Current == null)
                    Current = owner.Head;
                else
                    Current = Current.Next;
                return Current != null;
            }

            public void Reset() { Current = null; }

            private bool disposedValue = false;
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            protected virtual void Dispose(bool disposing)
            {
                if (!this.disposedValue)
                {
                    if (disposing) { }
                    Current = null;
                }
                this.disposedValue = true;
            }

            ~NodeEnumerator()
            {
                Dispose(false);
            }
        }

        public class ValueEnumerable : IEnumerable<T>
        {
            NodeList<T> owner;

            public ValueEnumerable(NodeList<T> owner) { this.owner = owner; }

            public IEnumerator<T> GetEnumerator() { return new ValueEnumerator(owner); }
            private IEnumerator GetEnumerator1() { return this.GetEnumerator(); }
            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator1(); }

        }

        public class ValueEnumerator : IEnumerator<T>
        {
            NodeList<T> owner;
            Node current;

            public ValueEnumerator(NodeList<T> owner) { this.owner = owner; current = null; }

            public T Current { get { return current.Value; } }
            private object Current1 { get { return this.Current; } }
            object IEnumerator.Current { get { return this.Current1; } }

            public bool MoveNext()
            {
                if (current == null)
                    current = owner.Head;
                else
                    current = current.Next;
                return current != null;
            }

            public void Reset() { current = null; }

            private bool disposedValue = false;
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            protected virtual void Dispose(bool disposing)
            {
                if (!this.disposedValue)
                {
                    if (disposing) { }
                    current = null;
                }
                this.disposedValue = true;
            }

            ~ValueEnumerator()
            {
                Dispose(false);
            }
        }
    }
}
