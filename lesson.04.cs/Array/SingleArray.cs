using System;
using System.Collections.Generic;
using System.Text;

namespace lesson._04.cs
{
    class SingleArray<T> : IArray<T>
    {
        private T[] data;

        public SingleArray()
        {
            data = new T[0];
        }

        public int Size()
        {
            return data.Length;
        }

        public void Add(T item)
        {
            Add(item, Size());
        }

        public void Add(T item, int index)
        {
            T[] newData = new T[data.Length + 1];
            Array.Copy(data, 0, newData, 0, index);
            if(index < data.Length)
                Array.Copy(data, index, newData, index + 1, data.Length - index);
            newData[index] = item;
            data = newData;
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

            T[] newData = new T[data.Length - 1];
            Array.Copy(data, 0, newData, 0, index);
            if (index < data.Length - 1)
                Array.Copy(data, index + 1, newData, index, data.Length - index - 1);
            data = newData;

            return item;
        }
    }
}
