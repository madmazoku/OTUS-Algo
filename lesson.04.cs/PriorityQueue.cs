using System;
using System.Collections;

namespace lesson._04.cs
{
    class PriorityQueue<T, U> : IEnumerable
        where U : IComparable<U>
    {
        struct PriorityNode
        {
            U priority;
            Queue<T> queue;

            public PriorityNode(U priority)
            {
                this.priority = priority;
                queue = new Queue<T>();
            }

            public U Priority { get { return priority; } }
            public Queue<T> Queue { get { return queue; } }
        }

        Queue<PriorityNode> priorityQueue;
        int size;

        public PriorityQueue()
        {
            priorityQueue = new Queue<PriorityNode>();
            size = 0;
        }

        public bool IsEmpty { get { return priorityQueue.IsEmpty; } }
        public int Size { get { return size; } }

        public void enque(U priority, T item)
        {
            ++size;
            Node<PriorityNode> prev = null;
            foreach (Node<PriorityNode> current in priorityQueue)
            {
                int cmp = priority.CompareTo(current.Item.Priority);
                if (cmp == 1)
                    break;
                else if (cmp == 0)
                {
                    current.Item.Queue.enque(item);
                    return;
                }
                prev = current;
            }
            PriorityNode newPriorityNode = new PriorityNode(priority);
            priorityQueue.enqueAfter(prev, newPriorityNode);
            newPriorityNode.Queue.enque(item);
        }

        public T deque()
        {
            if (IsEmpty)
                throw new Exception("empty collection");

            --size;

            T item = priorityQueue.Head.Item.Queue.deque();
            if (priorityQueue.Head.Item.Queue.IsEmpty)
                priorityQueue.deque();

            return item;
        }

        public T[] Array
        {
            get
            {
                T[] array = new T[size];
                int index = 0;
                foreach (T item in this)
                {
                    array[index] = item;
                    ++index;
                }
                return array;
            }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (Node<PriorityNode> priorityNode in priorityQueue)
                foreach (Node<T> node in priorityNode.Item.Queue)
                    yield return node.Item;
        }
    }
}
