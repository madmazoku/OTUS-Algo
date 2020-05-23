using System.Collections.Generic;
using System.Threading;

namespace lesson._06.cs
{
    class QuickTask : SortTask
    {
        public override string Name() { return "Quick"; }

        public override void Run(CancellationToken token)
        {
            QuickSort(sortArray, token);
        }

        static int PartArray(int[] array, int leftIndex, int rightIndex, CancellationToken token)
        {
            int pivotIndex = (leftIndex + rightIndex) >> 1;
            if (pivotIndex != leftIndex)
                Utils.Swap(array, leftIndex, pivotIndex, token);

            int pivot = array[leftIndex];
            int left = leftIndex;
            int right = rightIndex + 1;
            while (true)
            {
                while (array[++left] < pivot)
                    if (left == rightIndex)
                        break;
                while (array[--right] > pivot)
                    if (right == leftIndex)
                        break;
                if (left >= right)
                    break;
                Utils.Swap(array, left, right, token);
            }

            if (leftIndex != right)
                Utils.Swap(array, leftIndex, right, token);
            return right;
        }

        static void QuickSort(int[] array, CancellationToken token)
        {
            if (array.Length == 1)
                return;

            Stack<(int, int)> parts = new Stack<(int, int)>(100);
            parts.Push((0, array.Length - 1));
            while (parts.Count > 0)
            {
                (int leftIndex, int rightIndex) = parts.Pop();

                int midIndex = PartArray(array, leftIndex, rightIndex, token);

                if (midIndex - leftIndex < rightIndex - midIndex)
                {
                    if (midIndex + 1 < rightIndex)
                        parts.Push((midIndex + 1, rightIndex));
                    if (leftIndex < midIndex - 1)
                        parts.Push((leftIndex, midIndex - 1));
                }
                else
                {
                    if (midIndex + 1 < rightIndex)
                        parts.Push((midIndex + 1, rightIndex));
                    if (leftIndex < midIndex - 1)
                        parts.Push((leftIndex, midIndex - 1));
                }
            }
        }

    }
}
