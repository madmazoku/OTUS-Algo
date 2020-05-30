using System;
using System.Collections.Generic;

namespace lesson._08.cs
{
    class QuickMemorySort : IMemorySort
    {
        public string Name() { return "Quick"; }

        public void Sort(UInt16[] array, int start, int end)
        {
            QuickSort(array, start, end);
        }

        static int PartArray(UInt16[] array, int leftIndex, int rightIndex)
        {
            int pivotIndex = leftIndex + ((rightIndex - leftIndex + 1) >> 1);
            Utils.Swap(array, leftIndex, pivotIndex);

            UInt16 pivot = array[leftIndex];
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
                Utils.Swap(array, left, right);
            }

            Utils.Swap(array, leftIndex, right);
            return right;
        }

        static void QuickSort(UInt16[] array, int start, int end)
        {
            if (end - start <= 1)
                return;

            Stack<(int, int)> stack = new Stack<(int, int)>();
            stack.Push((start, end));
            while (stack.Count > 0)
            {
                (int startInner, int endInner) = stack.Pop();
                int p = PartArray(array, startInner, endInner - 1);
                if (p - startInner < endInner - p - 1)
                {
                    if (p + 2 < endInner)
                        stack.Push((p + 1, endInner));
                    if (startInner + 1 < p)
                        stack.Push((startInner, p));
                }
                else
                {
                    if (startInner + 1 < p)
                        stack.Push((startInner, p));
                    if (p + 2 < endInner)
                        stack.Push((p + 1, endInner));
                }
            }
        }

    }
}
