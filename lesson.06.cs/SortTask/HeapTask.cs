using System.Threading;

namespace lesson._06.cs
{
    class HeapTask : SortTask
    {
        public override string Name() { return "Heap"; }

        public override void Run(CancellationToken token)
        {
            HeapSort(sortArray, token);
        }

        static void Heapify(int[] array, int size, CancellationToken token)
        {
            if (size < 2)
                return;

            int root = (size >> 1) - 1;
            while (true)
            {
                SinkDown(array, root, size, token);
                if (root == 0)
                    break;
                --root;
            }
        }

        static void SinkDown(int[] array, int root, int size, CancellationToken token)
        {
            while (true)
            {
                int maxIndex = root;
                int leftIndex = (root << 1) + 1;
                int rightIndex = leftIndex + 1;
                if (leftIndex < size)
                {
                    if (array[maxIndex] < array[leftIndex])
                        maxIndex = leftIndex;
                    if (rightIndex < size && array[maxIndex] < array[rightIndex])
                        maxIndex = rightIndex;
                }
                if (maxIndex == root)
                    break;
                Utils.Swap(array, root, maxIndex, token);
                root = maxIndex;
            }
        }

        static void HeapSort(int[] array, CancellationToken token)
        {
            Heapify(array, array.Length, token);

            for (int size = array.Length; size > 0; --size)
            {
                SinkDown(array, 0, size, token);
                Utils.Swap(array, 0, size - 1, token);
            }
        }

    }
}
