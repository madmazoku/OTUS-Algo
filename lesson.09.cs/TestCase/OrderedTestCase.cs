using System;

namespace lesson._09.cs
{
    class OrderedTestCase : ITestCase
    {
        int arraySize;
        bool reverse;

        int[] insertArray;
        int[] findArray;
        int[] removeArray;

        public OrderedTestCase(int arraySize, bool reverse = false)
        {
            this.arraySize = arraySize;
            this.reverse = reverse;
        }

        public string Name()
        {
            int logArraySize = (int)Math.Ceiling(Math.Log10(arraySize));
            string type = reverse ? "Reversed" : "Ordered";
            return $"{type}.10^{logArraySize }";
        }

        public void Prepare()
        {
            insertArray = Utils.MakeIndexArray(arraySize);
            if (reverse)
                Array.Reverse(insertArray);
            int sampleSize = (arraySize / 10) + 1;
            findArray = Utils.Sample(insertArray, sampleSize);
            removeArray = Utils.Sample(insertArray, sampleSize);
        }

        public int[] GetInsertArray() { return insertArray; }
        public int[] GetFindArray() { return findArray; }
        public int[] GetRemoveArray() { return removeArray; }
    }
}
