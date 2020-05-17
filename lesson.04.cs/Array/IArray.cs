using System;
using System.Collections.Generic;
using System.Text;

namespace lesson._04.cs
{
    interface IArray<T>
    {
        public int Size();
        public void Add(T item);
        public void Add(T item, int index);
        public T Get(int index);
        public void Set(T item, int index);
        public T Remove(int index);
    }
}
