using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace lesson._04.cs
{
    class MatrixArray<T> : IArray<T>
    {
        private IArray<T[]> array;
        int size;
        int vector = 10;

        public MatrixArray()
        {
            array = new FactorArray<T[]>();
            array.Add(new T[vector]);
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
            if (size == array.Size() * vector)
                array.Add(new T[vector]);

            int slice = index / vector;
            int pos = index % vector;

            if (index < size)
            {
                int lastSlice = size / vector;
                int lastSize = size % vector;

                int currentSlice = lastSlice;
                T[] currentArray = array.Get(array.Size() - 1);
                if(currentSlice > slice)
                {
                    if (lastSize > 0)
                        Utils.MoveForward<T>(currentArray, 0, lastSize);
                    T[] prevArray = array.Get(currentSlice - 1);
                    currentArray[0] = prevArray[vector - 1];
                    currentArray = prevArray;
                    --currentSlice;
                    while(currentSlice > slice)
                    {
                        Utils.MoveForward<T>(currentArray, 0, vector - 1);
                        prevArray = array.Get(currentSlice - 1);
                        currentArray[0] = prevArray[vector - 1];
                        currentArray = prevArray;
                        --currentSlice;
                    }
                    if (pos < vector)
                        Utils.MoveForward<T>(currentArray, pos, vector - pos - 1);
                } else
                {
                    if(pos < lastSize)
                        Utils.MoveForward<T>(currentArray, pos, lastSize - pos);
                }
            }

            array.Get(slice)[pos] = item;
            ++size;
        }

        public T Get(int index)
        {
            return array.Get(index / vector)[index % vector];
        }

        public void Set(T item, int index)
        {
            array.Get(index / vector)[index% vector] = item;
        }

        public T Remove(int index)
        {
            T item = Get(index);

            int slice = index / vector;
            int pos = index % vector;

            if (index < size - 1)
            {
                int lastSlice = size / vector;
                int lastSize = size % vector;

                int currentSlice = slice;
                T[] currentArray = array.Get(currentSlice);
                if(currentSlice < lastSlice)
                {
                    if (pos < vector - 1)
                        Utils.MoveBackward<T>(currentArray, pos, vector - pos);
                    T[] nextArray = array.Get(currentSlice + 1);
                    currentArray[vector - 1] = nextArray[0];
                    currentArray = nextArray;
                    ++currentSlice;
                    while(currentSlice < lastSlice - 1)
                    {
                        Utils.MoveBackward<T>(currentArray, 0, vector);
                        nextArray = array.Get(currentSlice + 1);
                        currentArray[vector - 1] = nextArray[0];
                        currentArray = nextArray;
                        ++currentSlice;
                    }
                    if(lastSize > 0)
                        Utils.MoveBackward<T>(currentArray, 0, lastSize);
                    else
                        Utils.MoveBackward<T>(currentArray, 0, vector);
                }
                else
                {
                    if(pos < lastSize)
                        Utils.MoveBackward<T>(currentArray, pos, lastSize - pos);
                }
            }

            --size;

            return item;
        }
    }
}

