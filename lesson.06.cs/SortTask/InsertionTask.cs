using System.Threading;

namespace lesson._06.cs
{
    class InsertionTask : SortTask
    {
        public override string Name() { return "Insertion"; }

        public override void Run(CancellationToken token)
        {
            InsertionSort(sortArray, token);
        }

        static void InsertionSort(int[] array, CancellationToken token)
        {
            for (int first = 1; first < array.Length; ++first)
            {
                int index = first;
                int indexPrev = index - 1;
                while (array[indexPrev] > array[index])
                {
                    Utils.Swap(array, indexPrev, index, token);
                    if (indexPrev == 0)
                        break;
                    index = indexPrev;
                    --indexPrev;
                }
            }
        }
    }
}
