using System.Threading;

namespace lesson._06.cs
{
    class BubbleTask : SortTask
    {
        public override string Name() { return "Bubble"; }

        public override void Run(CancellationToken token)
        {
            BubbleSort(sortArray, token);
        }

        static void BubbleSort(int[] array, CancellationToken token)
        {
            bool swapped = true;
            int last = array.Length - 1;

            while (swapped && last > 0)
            {
                swapped = false;
                for (int index = 0; index < last; ++index)
                    if (array[index] > array[index + 1])
                    {
                        Utils.Swap(array, index, index + 1, token);
                        swapped = true;
                    }
                --last;
            }
        }
    }
}
