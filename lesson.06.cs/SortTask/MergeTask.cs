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

        static void Merge(int[] aux, int[] array, int start, int mid, int end, CancellationToken token)
        {
            Array.Copy(array, start, aux, start, mid - start);
            int left = start;
            int right = mid;
            int index = start;
            while (left < mid && right < end)
            {
                token.ThrowIfCancellationRequested();
                if (aux[left] < array[right])
                    array[index++] = aux[left++];
                else
                    array[index++] = array[right++];
            }
            Array.Copy(aux, left, array, index, mid - left);
        }

        static void MergeSort(int[] array, CancellationToken token)
        {
            int[] aux = new int[array.Length];
            for (int size = 1; size < array.Length; size <<= 1)
            {
                for (int start = 0; start < array.Length; start += (size << 1))
                {
                    int mid = start + size;
                    int end = mid + size;
                    if (mid < array.Length)
                    {
                        if (end > array.Length)
                            end = array.Length;
                        Merge(aux, array, array.Length - end, array.Length - mid, array.Length - start, token);
                    }
                }
            }
        }
    }
}
