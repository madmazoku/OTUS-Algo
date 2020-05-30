using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;

namespace lesson._08.cs
{
    class QuickMMFSort : IMMFSort
    {
        public string Name() { return "Quick"; }

        public void Sort(MemoryMappedViewAccessor mmva, long start, long end)
        {
            QuickSort(mmva, start, end);
        }

        static long PartArray(MemoryMappedViewAccessor mmva, long leftIndex, long rightIndex)
        {
            long pivotIndex = leftIndex + ((rightIndex - leftIndex + 1) >> 1);
            Utils.Swap(mmva, leftIndex, pivotIndex);

            UInt16 pivot = mmva.ReadUInt16(leftIndex * sizeof(UInt16));
            long left = leftIndex;
            long right = rightIndex + 1;
            while (true)
            {
                while (mmva.ReadUInt16((++left) * sizeof(UInt16)) < pivot)
                    if (left == rightIndex)
                        break;
                while (mmva.ReadUInt16((--right) * sizeof(UInt16)) > pivot)
                    if (right == leftIndex)
                        break;

                if (left >= right)
                    break;
                Utils.Swap(mmva, left, right);
            }

            Utils.Swap(mmva, leftIndex, right);
            return right;
        }

        static void QuickSort(MemoryMappedViewAccessor mmva, long start, long end)
        {
            if (end - start <= 1)
                return;

            Stack<(long, long)> stack = new Stack<(long, long)>();
            stack.Push((start, end));
            while (stack.Count > 0)
            {
                (long startInner, long endInner) = stack.Pop();
                long p = PartArray(mmva, startInner, endInner - 1);
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
