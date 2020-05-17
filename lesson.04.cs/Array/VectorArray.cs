using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace lesson._04.cs
{
    class VectorArray<T> : IArray<T>
    {
        private T[] data;
        int size;
        int vector = 10;

        public VectorArray()
        {
            data = new T[vector];
            size = 0;
        }

        public int Size()
        {
            return size;
        }

        public void Add(T item)
        {
            Add(item, Size());
        }

        public void Add(T item, int index)
        {
            if (size == data.Length)
            {
                T[] newData = new T[data.Length + vector];
                Array.Copy(data, 0, newData, 0, index);
                if (index < size)
                    Array.Copy(data, index, newData, index + 1, size - index);
                data = newData;
            } else
            {
                if(index < size)
                   Utils.MoveForward<T>(data, index, size - index);
            }
            data[index] = item;
            ++size;
        }

        public T Get(int index)
        {
            return data[index];
        }

        public void Set(T item, int index)
        {
            data[index] = item;
        }

        public T Remove(int index)
        {
            T item = data[index];
            Utils.MoveBackward<T>(data, index, size - index);
            --size;
            return item;
        }

    }
}
