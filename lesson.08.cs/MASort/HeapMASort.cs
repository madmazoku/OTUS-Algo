namespace lesson._08.cs
{
    class HeapMASort : IMASort
    {
        public string Name() { return "Heap"; }

        public void Sort(IMemoryAcessor ma, long start, long end)
        {
            HeapSort(ma, start, end);
        }

        static void Heapify(IMemoryAcessor ma, long start, long end)
        {
            if (end - start < 2)
                return;

            long root = ((end - start) >> 1) - 1;
            while (true)
            {
                SinkDown(ma, root, start, end);
                if (root == 0)
                    break;
                --root;
            }
        }

        static void SinkDown(IMemoryAcessor ma, long root, long start, long end)
        {
            long size = end - start;
            while (true)
            {
                long maxIndex = root;
                long leftIndex = (root << 1) + 1;
                long rightIndex = leftIndex + 1;
                if (leftIndex < size)
                {
                    if (ma.Read(start + maxIndex) < ma.Read(start + leftIndex))
                        maxIndex = leftIndex;
                    if (rightIndex < size && ma.Read(start + maxIndex) < ma.Read(start + rightIndex))
                        maxIndex = rightIndex;
                }
                if (maxIndex == root)
                    break;

                ma.Swap(start + maxIndex, start + root);

                root = maxIndex;
            }
        }

        static void HeapSort(IMemoryAcessor ma, long start, long end)
        {
            Heapify(ma, start, end);

            for (long size = end - start; size > 0; --size)
            {
                SinkDown(ma, 0, start, start + size);
                ma.Swap(start, start + size - 1);
            }
        }
    }
}
