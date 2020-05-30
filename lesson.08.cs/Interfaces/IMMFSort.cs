using System.IO.MemoryMappedFiles;

namespace lesson._08.cs
{
    interface IMMFSort
    {
        public string Name();
        public void Sort(MemoryMappedViewAccessor mmva, long start, long end);
    }
}
