using System;

namespace lesson._08.cs
{
    class MergeMemorySort : IMemorySort
    {
        int subSortSize;
        IMemorySort subSort;

        public MergeMemorySort(int subSortSize, IMemorySort subSort)
        {
            this.subSortSize = subSortSize;
            this.subSort = subSort;
        }

        public string Name() { return $"Merge.{subSort.Name()}"; }

        public void Sort(UInt16[] array, int start, int end)
        {
            MergeSort(array, start, end, subSortSize, subSort);
        }

        //public static void Swap(UInt16[] array, int leftIndex, int rightIndex)
        //{
        //    if (leftIndex == rightIndex)
        //        return;

        //    UInt16 t = array[leftIndex];
        //    array[leftIndex] = array[rightIndex];
        //    array[rightIndex] = t;
        //}

        //static int PartArray(UInt16[] array, int leftIndex, int rightIndex)
        //{
        //    int pivotIndex = leftIndex + ((rightIndex - leftIndex + 1) >> 1);
        //    Swap(array, leftIndex, pivotIndex);

        //    UInt16 pivot = array[leftIndex];
        //    int left = leftIndex;
        //    int right = rightIndex + 1;
        //    while (true)
        //    {
        //        while (array[++left] < pivot)
        //            if (left == rightIndex)
        //                break;
        //        while (array[--right] > pivot)
        //            if (right == leftIndex)
        //                break;

        //        if (left >= right)
        //            break;
        //        Swap(array, left, right);
        //    }

        //    Swap(array, leftIndex, right);
        //    return right;
        //}

        //static void QuickSort(UInt16[] array, int start, int end)
        //{
        //    if (end - start <= 1)
        //        return;

        //    Stack<(int, int)> stack = new Stack<(int, int)>();
        //    stack.Push((start, end));
        //    while (stack.Count > 0)
        //    {
        //        (int startInner, int endInner) = stack.Pop();
        //        int p = PartArray(array, startInner, endInner - 1);
        //        if (p - startInner < endInner - p - 1)
        //        {
        //            if (p + 2 < endInner)
        //                stack.Push((p + 1, endInner));
        //            if (startInner + 1 < p)
        //                stack.Push((startInner, p));
        //        }
        //        else
        //        {
        //            if (startInner + 1 < p)
        //                stack.Push((startInner, p));
        //            if (p + 2 < endInner)
        //                stack.Push((p + 1, endInner));
        //        }
        //    }
        //}

        static void Merge(UInt16[] aux, UInt16[] array, int start, int mid, int end)
        {
            Array.Copy(array, start, aux, start, mid - start);
            int left = start;
            int right = mid;
            int index = start;
            while (left < mid && right < end)
            {
                if (aux[left] < array[right])
                    array[index++] = aux[left++];
                else
                    array[index++] = array[right++];
            }
            Array.Copy(aux, left, array, index, mid - left);
        }

        static void MergeSort(UInt16[] array, int start, int end, int subSortSize, IMemorySort subSort)
        {
            UInt16[] aux = new UInt16[array.Length];

            for (int startInner = start; startInner < end; startInner += subSortSize)
            {
                int endInner = startInner + subSortSize;
                if (endInner > end)
                    endInner = end;
                subSort.Sort(array, end - endInner, end - startInner);
            }

            for (int size = subSortSize; size < end - start; size <<= 1)
            {
                for (int startInner = start; startInner < end; startInner += (size << 1))
                {
                    int midInner = startInner + size;
                    int endInner = midInner + size;
                    if (midInner < end)
                    {
                        if (endInner > end)
                            endInner = end;
                        Merge(aux, array, end - endInner, end - midInner, end - startInner);
                    }
                }
            }
        }

    }
}
