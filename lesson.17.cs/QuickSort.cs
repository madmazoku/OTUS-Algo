using lesson._16.cs;
using System;

namespace lesson._17.cs
{
    public class QuickSort<T>
    {
        static public void Sort(T[] array, Func<T, T, int> compare, int? from = null, int? to = null)
        {
            if (array.Length <= 1)
                return;

            NodeStack<(int, int)> stack = new NodeStack<(int, int)>();
            stack.Push((from ?? 0, to ?? array.Length));
            while (stack.size > 0)
            {
                (int start, int end) = stack.Pop();
                int p = PartArray(array, start, end - 1, compare);
                if (p - start < end - p - 1)
                {
                    if (p + 2 < end)
                        stack.Push((p + 1, end));
                    if (start + 1 < p)
                        stack.Push((start, p));
                }
                else
                {
                    if (start + 1 < p)
                        stack.Push((start, p));
                    if (p + 2 < end)
                        stack.Push((p + 1, end));
                }
            }
        }

        static int PartArray(T[] array, int leftIndex, int rightIndex, Func<T, T, int> compare)
        {
            int pivotIndex = leftIndex + ((rightIndex - leftIndex + 1) >> 1);
            (array[leftIndex], array[pivotIndex]) = (array[pivotIndex], array[leftIndex]);

            T pivot = array[leftIndex];
            int left = leftIndex;
            int right = rightIndex + 1;
            while (true)
            {
                while (compare(array[++left], pivot) < 0)
                    if (left == rightIndex)
                        break;
                while (compare(array[--right], pivot) > 0)
                    if (right == leftIndex)
                        break;

                if (left >= right)
                    break;
                (array[left], array[right]) = (array[right], array[left]);
            }

            (array[leftIndex], array[right]) = (array[right], array[leftIndex]);

            return right;
        }

    }
}
