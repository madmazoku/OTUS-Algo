using System;
using System.Threading;

namespace lesson._08.cs
{
    interface IMemoryAcessor
    {
        public long Size();
        public IMemoryAcessor CloneAUX();

        public UInt16 Read(long index);
        public void Write(long index, UInt16 value);
        public void Swap(long left, long right, CancellationToken token);
    }
}
