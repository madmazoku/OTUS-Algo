using System;
using System.Threading;

namespace lesson._08.cs
{
    class ArrayMemoryAccessor : IMemoryAcessor
    {
        UInt16[] array;

        public ArrayMemoryAccessor(UInt16[] array)
        {
            this.array = array;
        }

        public ArrayMemoryAccessor(long size)
        {
            this.array = new UInt16[size];
        }

        public long Size()
        {
            return array.Length;
        }

        public IMemoryAcessor CloneAUX()
        {
            return new ArrayMemoryAccessor(array.Length);
        }

        public UInt16 Read(long index)
        {
            return array[index];

        }
        public void Write(long index, UInt16 value)
        {
            array[index] = value;
        }

        public void Swap(long left, long right, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (left == right)
                return;

            UInt16 t = array[left];
            array[left] = array[right];
            array[right] = t;
        }
    }
}
