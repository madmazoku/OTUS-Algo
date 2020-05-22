using System;
using System.Threading;

namespace lesson._06.cs
{
    abstract class SortTask : ITestTask
    {
        protected int[] sortArray;

        public abstract string Name();

        public void Prepare(ITestCase testCase)
        {
            SortCase sortCase = (SortCase)testCase;
            sortArray = new int[sortCase.sourceArray.Length];
            Array.Copy(sortCase.sourceArray, sortArray, sortCase.sourceArray.Length);
        }

        public abstract void Run(CancellationToken token);

        public bool Compare(ITestCase testCase)
        {
            SortCase sortCase = (SortCase)testCase;
            if (sortCase.expectArray.Length != sortArray.Length)
                return false;
            for (int index = 0; index < sortCase.expectArray.Length; ++index)
                if (sortCase.expectArray[index] != sortArray[index])
                    return false;
            return true;
        }

    }
}
