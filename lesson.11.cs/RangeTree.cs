using System;

namespace lesson._11.cs
{
    class RangeTree
    {
        int[] array;
        long[] preArray;
        Func<long, long, long> operand;
        int identity;

        static readonly double log2 = Math.Log(2);

        public RangeTree(int size, Func<long, long, long> operand, int identity)
        {
            preArray = new long[1 << (int)Math.Ceiling(Math.Log(size) / log2)];
            array = new int[size];

            this.operand = operand;
            this.identity = identity;
        }

        public RangeTree(int[] array, Func<long, long, long> operand, int identity)
        {
            preArray = new long[1 << (int)Math.Ceiling(Math.Log(array.Length) / log2)];

            this.array = array;
            this.operand = operand;
            this.identity = identity;

            Refresh();
        }

        void Refresh()
        {
            int index = preArray.Length - 1;
            int index2 = (index << 1) - preArray.Length;
            while (index2 >= 0)
            {
                int leftValue = index2 < array.Length ? array[index2] : identity;
                int rightValue = index2 + 1 < array.Length ? array[index2 + 1] : identity;

                preArray[index] = operand(leftValue, rightValue);

                index -= 1;
                index2 -= 2;
            }

            index2 = index << 1;

            while (index > 0)
            {
                preArray[index] = operand(preArray[index2], preArray[index2 + 1]);

                index -= 1;
                index2 -= 2;
            }
        }

        public void SetAt(int index, int value)
        {
            index -= 1;
            array[index] = value;

            index &= ~0x01;
            long leftValue = array[index];
            long rightValue = index + 1 < array.Length ? array[index + 1] : identity;
            index = (index + preArray.Length) >> 1;
            preArray[index] = operand(leftValue, rightValue);

            while (index > 1)
            {
                index &= ~0x01;
                leftValue = preArray[index];
                rightValue = preArray[index + 1];
                index >>= 1;
                preArray[index] = operand(leftValue, rightValue);
            }
        }

        public long GetRange(int left, int right)
        {
            left -= 1;
            right -= 1;

            long result = identity;

            if ((left & 0x01) == 1)
                result = operand(array[left], result);
            left = (left + preArray.Length + 1) >> 1;
            if ((right & 0x01) == 0)
                result = operand(result, array[right]);
            right = (right + preArray.Length - 1) >> 1;

            while (left <= right)
            {
                if ((left & 0x01) == 1)
                    result = operand(preArray[left], result);
                left = (left + 1) >> 1;
                if ((right & 0x01) == 0)
                    result = operand(result, preArray[right]);
                right = (right - 1) >> 1;
            }

            return result;
        }

    }

}
