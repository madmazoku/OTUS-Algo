using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

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

        static (bool, double) BenchmarkSort(string name, Func<int[], int[]> sortFunc, int[] array)
        {
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

        static void Heapify(int[] array, int index, int max)
        {
            int maxIndex = index;
            int leftIndex = (index << 1) + 1;

            if (leftIndex < max)
            {
                Heapify(array, leftIndex, max);
                if (array[leftIndex] > array[maxIndex])
                    maxIndex = leftIndex;

                int rightIndex = leftIndex + 1;
                if (rightIndex < max)
                {
                    Heapify(array, rightIndex, max);
                    if (array[rightIndex] > array[maxIndex])
                        maxIndex = rightIndex;
                }

                if (index != maxIndex)
                {
                    Swap(ref array[index], ref array[maxIndex]);
                    Heapify(array, maxIndex, max);
                }
            }
        }

        static int[] FullHeapSort(int[] array)
        {
            for (int max = array.Length; max > 1; --max)
            {
                Heapify(array, 0, max);
                Swap(ref array[0], ref array[max - 1]);
            }
            return array;
        }

        static void SinkDown(int[] array, int index, int max)
        {
            int maxIndex, leftIndex, rightIndex;
            while (index < max)
            {
                maxIndex = index;
                leftIndex = (index << 1) + 1;
                rightIndex = leftIndex + 1;

                if (leftIndex < max && array[maxIndex] < array[leftIndex])
                    maxIndex = leftIndex;
                if (rightIndex < max && array[maxIndex] < array[rightIndex])
                    maxIndex = rightIndex;
                if (index == maxIndex)
                    return;
                Swap(ref array[index], ref array[maxIndex]);
                index = maxIndex;
            }

        }

        static int[] SinkHeapSort(int[] array)
        {
            for (int max = (array.Length >> 1); max > 0; --max)
                SinkDown(array, max - 1, array.Length);

            for (int max = array.Length; max > 1; --max)
            {
                Swap(ref array[0], ref array[max - 1]);
                SinkDown(array, 0, max - 1);
            }
            return array;
        }

        static int[] PartialHeapSort(int[] array)
        {
            for (int max = array.Length; max > 0; --max)
            {
                for (int index = (max >> 1); index > 0; --index)
                {
                    int realIndex = index - 1;
                    int maxIndex = realIndex;
                    int leftIndex = (realIndex << 1) + 1;
                    if (leftIndex < max)
                    {
                        if (array[maxIndex] < array[leftIndex])
                            maxIndex = leftIndex;

                        int rightIndex = leftIndex + 1;
                        if (leftIndex < max - 1 && array[maxIndex] < array[rightIndex])
                            maxIndex = rightIndex;
                    }
                    if (maxIndex != realIndex)
                        Swap(ref array[realIndex], ref array[maxIndex]);
                }
                Swap(ref array[0], ref array[max - 1]);
            }
            return array;
        }

        class SortTestCase
        {
            public string name;
            public Func<int[], int[]> sortFunc;
            public Task<(bool, double)> result;

            public SortTestCase(string name, Func<int[], int[]> sortFunc)
            {
                this.name = name;
                this.sortFunc = sortFunc;
                this.result = null;
            }
        }

        static int POW(int x, int y)
        {
            int r = 1;
            while (y > 1)
            {
                if ((y & 0x1) == 1)
                    r *= x;
                x *= x;
                y >>= 1;
            }
            if (y > 0)
                r *= x;
            return r;
        }

        static void Main(string[] args)
        {
            int[] arr = RandomizeArray(10);
            SinkHeapSort(arr);

            List<SortTestCase> testCases = new List<SortTestCase>();
            testCases.Add(new SortTestCase("Bubble", BubbleSort));
            testCases.Add(new SortTestCase("Selection", SelectionSort));
            testCases.Add(new SortTestCase("Insertion", InsertionSort));
            testCases.Add(new SortTestCase("Shell", ShellSort));
            testCases.Add(new SortTestCase("Full Heap", FullHeapSort));
            testCases.Add(new SortTestCase("Sink Heap", SinkHeapSort));
            testCases.Add(new SortTestCase("Partial Heap", PartialHeapSort));

            Console.Write($"{"Array",12} |");
            foreach (SortTestCase testCase in testCases)
                Console.Write($" {testCase.name,12} |");
            Console.WriteLine("");

            int allAttempts = 10;
            for (int attempt = 0; attempt < allAttempts; ++attempt)
            {
                int[] array = RandomizeArray(POW(10, attempt + 1));
                Console.Write($"{array.Length,12} |");
                foreach (SortTestCase testCase in testCases)
                    testCase.result = Task.Run(() =>
                    {
                        Task<(bool, double)> result = Task.Run(() => { return BenchmarkSort(testCase.name, testCase.sortFunc, array); });
                        if (result.Wait(TimeSpan.FromSeconds(60)))
                            return result.Result;
                        else
                            return (false, 0);
                    });
                foreach (SortTestCase testCase in testCases)
                {
                    testCase.result.Wait();
                    (bool success, double avgSec) = testCase.result.Result;
                    if (!success)
                        Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write($" {avgSec,12:g7} |");
                    if (!success)
                        Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
            Console.ReadKey();
        }
    }
}
