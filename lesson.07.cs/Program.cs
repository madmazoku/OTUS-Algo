using System;
using System.Collections.Generic;

namespace lesson._07.cs
{
    class Program
    {
        static void Swap(int[] array, int left, int right)
        {
            int t = array[left];
            array[left] = array[right];
            array[right] = t;
        }

        static int Part(int[] array, int start, int end)
        {
            int pivot = array[end - 1];
            int a = start - 1;
            for (int m = start; m < end; ++m)
                if (array[m] <= pivot)
                    if (++a != m)
                        Swap(array, a, m);
            return a;
        }

        static void QuickSort(int[] array)
        {
            Stack<(int, int)> stack = new Stack<(int, int)>();
            stack.Push((0, array.Length));
            while (stack.Count > 0)
            {
                (int start, int end) = stack.Pop();
                if (start < end)
                {
                    int p = Part(array, start, end);
                    stack.Push((start, p));
                    stack.Push((p + 1, end));
                }
            }
        }

        static void Merge(int[] aux, int[] array, int start, int mid, int end)
        {
            Console.WriteLine($"Merge({start}, {mid}, {end})");
            Array.Copy(array, start, aux, start, mid - start);
            int left = start;
            int right = mid;
            int index = start;
            while (left < mid && right < end)
                if (aux[left] < array[right])
                    array[index++] = aux[left++];
                else
                    array[index++] = array[right++];
            while (left < mid)
                array[index++] = aux[left++];
        }

        static void MergeSort(int[] array)
        {
            int[] aux = new int[array.Length];
            for (int size = 1; size < array.Length; size <<= 1)
            {
                for (int start = 0; start < array.Length; start += (size << 1))
                {
                    int mid = start + size;
                    int end = mid + size;
                    if (mid > array.Length)
                        mid = array.Length;
                    if (end > array.Length)
                        end = array.Length;
                    if (mid < end)
                        Merge(aux, array, array.Length - end, array.Length - mid, array.Length - start);
                    PrintArray("merged", array);
                }
                Console.WriteLine("");
            }
            //_MergeSort(new int[array.Length], array, 0, array.Length);
        }

        static void _MergeSort(int[] aux, int[] array, int start, int end)
        {
            if (end - start <= 1)
                return;

            int mid = start + ((end - start) >> 1);
            _MergeSort(aux, array, start, mid);
            _MergeSort(aux, array, mid, end);
            Merge(aux, array, start, mid, end);
        }

        static void PrintArray(string name, int[] array)
        {
            Console.Write($"{name,20} [{array.Length}]");
            for (int index = 0; index < array.Length; ++index)
                Console.Write($"; {array[index]}");
            Console.WriteLine("");
        }

        static void Main(string[] args)
        {
            int[] array = { 2, 7, 0, 3, 9, 6, 4, 5, 7 };
            PrintArray("start", array);
            MergeSort(array);
            PrintArray("end", array);

            Console.WriteLine($"END");
        }
    }
}
