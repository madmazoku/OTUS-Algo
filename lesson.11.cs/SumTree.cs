using System;

namespace lesson._11.cs
{
    class SumTree
    {
        int length;
        long[] array;

        static readonly double log2 = Math.Log(2);

        public SumTree(int size)
        {
            length = 1 << (int)Math.Ceiling(Math.Log(size) / log2);
            array = new long[length << 1];
        }

        public void SetAt(int index, int value)
        {
            index = index - 1 + length;
            array[index] = value;

            while (index > 1)
            {
                index &= ~0x01;
                long sum = array[index] + array[index + 1];
                index >>= 1;
                array[index] = sum;
            }
        }

        public long GetRange(int left, int right)
        {
            left = left - 1 + length;
            right = right - 1 + length;

            long sum = 0;

            while (left <= right)
            {
                if ((left & 0x01) == 1)
                    sum += array[left];
                left = (left + 1) >> 1;
                if ((right & 0x01) == 0)
                    sum += array[right];
                right = (right - 1) >> 1;
            }

            return sum;
        }

    }

}
