using System;
using System.Threading;

namespace lesson._06.cs
{
    class MergeTask : SortTask
    {
        public override string Name() { return "Merge"; }

        public override void Run(CancellationToken token)
        {
            MergeSort(sortArray, token);
        }

        static void Merge(int[] tmpArray, int[] array, int start, int mid, int end, CancellationToken token)
        {
            int mergeIdnex = start;
            int left = start;
            int right = mid;

            Array.Copy(array, start, tmpArray, start, end - start);

            do
            {
                if (left < mid && (right == end || tmpArray[left] < tmpArray[right]))
                {
                    array[mergeIdnex] = tmpArray[left];
                    ++left;
                }
                else
                {
                    array[mergeIdnex] = tmpArray[right];
                    ++right;
                }
                ++mergeIdnex;
            } while (mergeIdnex < end);

        }

        static void Sort(int[] tmpArray, int[] array, int start, int end, CancellationToken token)
        {
            if (start + 1 == end)
                return;

            int mid = (start + end) >> 1;
            Sort(tmpArray, array, start, mid, token);
            Sort(tmpArray, array, mid, end, token);
            Merge(tmpArray, array, start, mid, end, token);
        }

        static void MergeSort(int[] array, CancellationToken token)
        {
            int[] tmpArray = new int[array.Length];

            Sort(tmpArray, array, 0, array.Length, token);
        }
    }
}
