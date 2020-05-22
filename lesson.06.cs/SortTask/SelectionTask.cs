using System.Threading;

namespace lesson._06.cs
{
    class SelectionTask : SortTask
    {
        public override string Name() { return "Selection"; }

        public override void Run(CancellationToken token)
        {
            SelectionSort(sortArray, token);
        }

        static void SelectionSort(int[] array, CancellationToken token)
        {
            for (int first = 0; first < array.Length - 1; ++first)
            {
                int min_index = first;
                int min_value = array[min_index];
                for (int index = first + 1; index < array.Length; ++index)
                    if (array[index] < min_value)
                    {
                        min_index = index;
                        min_value = array[index];
                    }
                if (first != min_index)
                    Utils.Swap(array, first, min_index, token);
            }
        }
    }
}
