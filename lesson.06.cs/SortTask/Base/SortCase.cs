using System;
using System.Linq;

namespace lesson._06.cs
{
    class SortCase : ITestCase
    {
        public int[] sourceArray;
        public int[] expectArray;

        public SortCase()
        {
            sourceArray = null;
            expectArray = null;
        }

        public void Prepare(string[] given, string[] expect)
        {
            int N = int.Parse(given[0]);
            sourceArray = given[1].Split(' ').Select((x) => int.Parse(x)).ToArray();
            if (N != sourceArray.Length)
                throw new Exception("Invalid array size");
            expectArray = expect[0].Split(' ').Select((x) => int.Parse(x)).ToArray();
            if (sourceArray.Length != expectArray.Length)
                throw new Exception("Mismatch source-expect array sizes");
        }
    }
}
