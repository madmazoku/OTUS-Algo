using System;

namespace lesson._08.cs
{
    interface IMemorySort
    {
        public string Name();
        public void Sort(UInt16[] array, int start, int end);
    }
}
