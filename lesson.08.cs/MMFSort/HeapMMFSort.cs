using System;
using System.IO.MemoryMappedFiles;

namespace lesson._08.cs
{
    class HeapMMFSort : IMMFSort
    {
        public string Name() { return "Heap"; }

        public void Sort(MemoryMappedViewAccessor mmva, long start, long end)
        {
            HeapSort(mmva, start, end);
        }

        static void Heapify(MemoryMappedViewAccessor mmva, long start, long end)
        {
            if (end - start < 2)
                return;

            long root = ((end - start) >> 1) - 1;
            while (true)
            {
                SinkDown(mmva, root, start, end);
                if (root == 0)
                    break;
                --root;
            }
        }

        static void SinkDown(MemoryMappedViewAccessor mmva, long root, long start, long end)
        {
            long size = end - start;
            while (true)
            {
                long maxIndex = root;
                long leftIndex = (root << 1) + 1;
                long rightIndex = leftIndex + 1;
                if (leftIndex < size)
                {
                    if (mmva.ReadUInt16((start + maxIndex) * sizeof(UInt16)) < mmva.ReadUInt16((start + leftIndex) * sizeof(UInt16)))
                        maxIndex = leftIndex;
                    if (rightIndex < size && mmva.ReadUInt16((start + maxIndex) * sizeof(UInt16)) < mmva.ReadUInt16((start + rightIndex) * sizeof(UInt16)))
                        maxIndex = rightIndex;
                }
                if (maxIndex == root)
                    break;

                Utils.Swap(mmva, start + maxIndex, start + root);

                root = maxIndex;
            }
        }

        static void HeapSort(MemoryMappedViewAccessor mmva, long start, long end)
        {
            Heapify(mmva, start, end);

            for (long size = end - start; size > 0; --size)
            {
                SinkDown(mmva, 0, start, start + size);
                Utils.Swap(mmva, start, start + size - 1);
            }
        }
    }
}
