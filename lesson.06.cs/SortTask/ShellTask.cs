using System.Threading;

namespace lesson._06.cs
{
    class ShellTask : SortTask
    {
        IShellSequence sequence;

        public ShellTask(IShellSequence sequence)
        {
            this.sequence = sequence;
        }

        public override string Name() { return $"Shell {sequence.Name()}"; }

        public override void Run(CancellationToken token)
        {
            ShellSort(sortArray, sequence, token);
        }

        static void ShellSort(int[] array, IShellSequence sequence, CancellationToken token)
        {
            int gap = sequence.Initial(array.Length);

            while (true)
            {
                for (int first = gap; first < array.Length; ++first)
                {
                    int index = first;
                    int indexPrev = index - gap;
                    while (array[indexPrev] > array[index])
                    {
                        Utils.Swap(array, indexPrev, index, token);
                        if (indexPrev < gap)
                            break;
                        index = indexPrev;
                        indexPrev -= gap;
                    }
                }

                gap = sequence.Decrease(gap);
                if (gap == 0)
                    break;
            }
        }
    }
}
