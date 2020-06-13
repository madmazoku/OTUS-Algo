using System;

namespace lesson._09.cs
{
    class RandomTestCase : ITestCase
    {
        int arraySize;

        int[] insertArray;
        int[] findArray;
        int[] removeArray;

        public RandomTestCase(int arraySize)
        {
            this.arraySize = arraySize;
        }

        public string Name()
        {
            int logArraySize = (int)Math.Ceiling(Math.Log10(arraySize));
            return $"Random.10^{logArraySize }";
        }

        public void Prepare()
        {
            insertArray = Utils.MakeIndexArray(arraySize);
            Utils.Shuffle(insertArray);
            int sampleSize = (arraySize / 10) + 1;
            findArray = Utils.Sample(insertArray, sampleSize);
            removeArray = Utils.Sample(insertArray, sampleSize);
        }

        public int[] GetInsertArray() { return insertArray; }
        public int[] GetFindArray() { return findArray; }
        public int[] GetRemoveArray() { return removeArray; }
    }
}
