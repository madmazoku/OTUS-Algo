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
            int pivotIndex = leftIndex + ((rightIndex - leftIndex + 1) >> 1);
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

            Utils.Swap(array, leftIndex, right, token);
            return right;
        }

        static void QuickSort(int[] array, CancellationToken token)
        {
            if (array.Length <= 1)
                return;

            Stack<(int, int)> stack = new Stack<(int, int)>();
            stack.Push((0, array.Length));
            while (stack.Count > 0)
            {
                (int start, int end) = stack.Pop();
                int p = PartArray(array, start, end - 1, token);
                if (p - start < end - p - 1)
                {
                    if (p + 2 < end)
                        stack.Push((p + 1, end));
                    if (start + 1 < p)
                        stack.Push((start, p));
                }
                else
                {
                    if (start + 1 < p)
                        stack.Push((start, p));
                    if (p + 2 < end)
                        stack.Push((p + 1, end));
                }
            }
        }

    }
}
