using System;
using System.Diagnostics;

namespace lesson._05.cs
{
    class Program
    {
        static void Swap(ref int x, ref int y)
        {
            int t = x;
            x = y;
            y = t;
        }

        static int[] RandomizeArray(int n)
        {
            int[] array = new int[n];
            Random rnd = new Random();
            for (int index = 0; index < n; ++index)
                array[index] = (int)(rnd.NextDouble() * n);
            return array;
        }

        static int[] CopyArray(int[] array)
        {
            int[] newArray = new int[array.Length];
            Array.Copy(array, newArray, array.Length);
            return newArray;
        }

        static bool CheckSort(int[] array)
        {
            for (int index = 0; index < array.Length - 1; ++index)
                if (array[index] > array[index + 1])
                    return false;
            return true;
        }

        static (bool, double) BenchmarkSort(string name, int[] array, Func<int[], int[]> sortFunc)
        {
            //Console.WriteLine($"\t{name} Sort check");
            double sec = 0;
            int[] testArray = new int[array.Length];
            int cnt = 0;
            while (sec < 1)
            {
                ++cnt;
                Array.Copy(array, testArray, array.Length);
                Stopwatch sw = Stopwatch.StartNew();
                sortFunc(testArray);
                sw.Stop();
                sec += sw.Elapsed.TotalSeconds;
            }
            double avgSec = sec / cnt;
            bool success = CheckSort(testArray);
            //Console.WriteLine($"\t\tSorting: {success}; Average time: {avgSec} sec");
            if (!success) throw new Exception("Not sorting properly");
            return (success, avgSec);
        }

        static int[] BubbleSort(int[] array)
        {
            bool swapped = true;
            int last = array.Length - 1;

            while (swapped && last > 0)
            {
                swapped = false;
                for (int index = 0; index < last; ++index)
                    if (array[index] > array[index + 1])
                    {
                        Swap(ref array[index], ref array[index + 1]);
                        swapped = true;
                    }
                --last;
            }
            return array;
        }

        static int[] SelectionSort(int[] array)
        {
            for (int first = 0; first < array.Length - 1; ++first)
            {
                int min_index = first;
                int min_value = array[min_index];
                for (int index = first + 1; index < array.Length; ++index)
                {
                    if (array[index] < min_value)
                    {
                        min_index = index;
                        min_value = array[index];
                    }
                }
                if (first != min_index)
                    Swap(ref array[first], ref array[min_index]);
            }
            return array;
        }

        static int[] InsertionSort(int[] array)
        {
            for (int first = 1; first < array.Length; ++first)
            {
                for (int index = first; index > 0; --index)
                    if (array[index - 1] > array[index])
                        Swap(ref array[index - 1], ref array[index]);
                    else
                        break;
            }
            return array;
        }

        static int[] InsertionSort(int[] array, int gap, int offset)
        {
            int items = array.Length / gap;
            for (int first = 1; first < items; ++first)
            {
                for (int index = first; index > 0; --index)
                {
                    int realIndex = index * gap + offset;
                    int realIndexPrev = realIndex - gap;
                    if (array[realIndexPrev] > array[realIndex])
                        Swap(ref array[realIndexPrev], ref array[realIndex]);
                    else
                        break;
                }
            }
            return array;
        }

        static double phi = (Math.Sqrt(5) + 1) / 2;
        static int[] ShellSort(int[] array)
        {
            int gap = array.Length >> 1;
            while (gap > 1)
            {
                for (int offset = 0; offset < gap; ++offset)
                    InsertionSort(array, gap, offset);
                gap >>= 1;
            }
            InsertionSort(array);
            return array;
        }

        static void Main(string[] args)
        {
            double bubbleAvgSec = 0;
            double selectionAvgSec = 0;
            double insertionAvgSec = 0;
            double shellAvgSec = 0;
            int allAttempts = 10;
            for (int attempt = 0; attempt < allAttempts; ++attempt)
            {
                Console.WriteLine($"Try attempt: {attempt}");
                int[] array = RandomizeArray(100000);
                {
                    (bool success, double avgSec) = BenchmarkSort("Bubble", array, BubbleSort);
                    bubbleAvgSec += avgSec;
                }
                {
                    (bool success, double avgSec) = BenchmarkSort("Selection", array, SelectionSort);
                    selectionAvgSec += avgSec;
                }
                {
                    (bool success, double avgSec) = BenchmarkSort("Insertion", array, InsertionSort);
                    insertionAvgSec += avgSec;
                }
                {
                    (bool success, double avgSec) = BenchmarkSort("Shell", array, ShellSort);
                    shellAvgSec += avgSec;
                }
            }
            bubbleAvgSec /= allAttempts;
            selectionAvgSec /= allAttempts;
            insertionAvgSec /= allAttempts;
            shellAvgSec /= allAttempts;
            Console.WriteLine("");

            Console.WriteLine($"Bubble    sorting average time: {bubbleAvgSec} sec");
            Console.WriteLine($"Selection sorting average time: {selectionAvgSec} sec");
            Console.WriteLine($"Insertion sorting average time: {insertionAvgSec} sec");
            Console.WriteLine($"Shell sorting average time: {shellAvgSec} sec");
        }
    }
}
