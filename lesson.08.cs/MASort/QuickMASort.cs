using System;
using System.Collections.Generic;

namespace lesson._08.cs
{
    class QuickMASort : IMASort
    {
        public string Name() { return "Quick"; }

        public void Sort(IMemoryAcessor ma, long start, long end)
        {
            QuickSort(ma, start, end);
        }

        static long PartArray(IMemoryAcessor ma, long leftIndex, long rightIndex)
        {
            long pivotIndex = leftIndex + ((rightIndex - leftIndex + 1) >> 1);
            ma.Swap(leftIndex, pivotIndex);

            UInt16 pivot = ma.Read(leftIndex);
            long left = leftIndex;
            long right = rightIndex + 1;
            while (true)
            {
                while (ma.Read(++left) < pivot)
                    if (left == rightIndex)
                        break;
                while (ma.Read(--right) > pivot)
                    if (right == leftIndex)
                        break;

                if (left >= right)
                    break;
                ma.Swap(left, right);
            }

            ma.Swap(leftIndex, right);
            return right;
        }

        static void QuickSort(IMemoryAcessor ma, long start, long end)
        {
            if (end - start <= 1)
                return;

            Stack<(long, long)> stack = new Stack<(long, long)>();
            stack.Push((start, end));
            while (stack.Count > 0)
            {
                (long startInner, long endInner) = stack.Pop();
                long p = PartArray(ma, startInner, endInner - 1);
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
