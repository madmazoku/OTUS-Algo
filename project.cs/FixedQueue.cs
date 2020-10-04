using System;

namespace project.cs
{
    class FixedQueue<T>
    {
        int start;
        int end;
        T[] data;

        public int Count { get { return end - start; } }

        public FixedQueue(int size)
        {
            data = new T[size];
        }

        public void Enqueue(T item)
        {
            data[end++] = item;
        }

        public T Dequeue()
        {
            return data[start++];
        }

        public void Clear()
        {
            start = end = 0;
        }

        public T[] ToArray()
        {
            int size = end - start;
            T[] array = new T[size];
            Array.Copy(data, start, array, 0, size);
            return array;
        }
    }
}
