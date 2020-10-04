using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace project.cs
{
    class FixedStack<T>
    {
        int pos;
        T[] data;

        public int Count { get { return pos; } }

        public FixedStack(int size)
        {
            data = new T[size];
            pos = size;
        }

        public void Push(T item)
        {
            data[--pos] = item;
        }

        public T Pop()
        {
            return data[pos++];
        }

        public T Peek()
        {
            return data[pos];
        }

        public void Clear()
        {
            pos = data.Length;
        }

        public T[] ToArray()
        {
            int size = data.Length - pos;
            T[] array = new T[size];
            Array.Copy(data, pos, array, 0, size);
            return array;
        }
    }
}
