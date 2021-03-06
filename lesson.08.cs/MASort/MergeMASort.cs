﻿using System.Threading;

namespace lesson._08.cs
{
    class MergeMASort : IMASort
    {
        int subSortSize;
        IMASort subSort;

        public MergeMASort(int subSortSize, IMASort subSort)
        {
            this.subSortSize = subSortSize;
            this.subSort = subSort;
        }

        public string Name() { return $"Merge.{subSort.Name()}"; }

        public void Sort(IMemoryAcessor ma, long start, long end, CancellationToken token)
        {
            MergeSort(ma, start, end, subSortSize, subSort, token);
        }

        static void Merge(IMemoryAcessor aux, IMemoryAcessor ma, long start, long mid, long end, CancellationToken token)
        {
            Utils.Copy(ma, start, aux, start, mid - start, token);
            long left = start;
            long right = mid;
            long index = start;
            while (left < mid && right < end)
            {
                token.ThrowIfCancellationRequested();
                if (aux.Read(left) < ma.Read(right))
                    ma.Write(index++, aux.Read(left++));
                else
                    ma.Write(index++, ma.Read(right++));
            }
            Utils.Copy(aux, left, ma, index, mid - left, token);
        }

        static void MergeSort(IMemoryAcessor ma, long start, long end, long subSortSize, IMASort subSort, CancellationToken token)
        {
            IMemoryAcessor aux = ma.CloneAUX();

            for (long startInner = start; startInner < end; startInner += subSortSize)
            {
                long endInner = startInner + subSortSize;
                if (endInner > end)
                    endInner = end;
                subSort.Sort(ma, end - endInner, end - startInner, token);
            }

            for (long size = subSortSize; size < end - start; size <<= 1)
            {
                for (long startInner = start; startInner < end; startInner += (size << 1))
                {
                    long midInner = startInner + size;
                    long endInner = midInner + size;
                    if (midInner < end)
                    {
                        if (endInner > end)
                            endInner = end;
                        Merge(aux, ma, end - endInner, end - midInner, end - startInner, token);
                    }
                }
            }
        }

    }
}
