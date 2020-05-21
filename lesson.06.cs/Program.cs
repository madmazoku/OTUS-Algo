using System;

namespace lesson._06.cs
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

        static void PrintArray(string name, int[] array)
        {
            Console.Write($"{name, 20} | array[{array.Length, 3}] |");
            for (int index = 0; index < array.Length; ++index)
                Console.Write($" {index}:{array[index], 3} |");
            Console.WriteLine("");
        }

        static void FindMax(int[] array, int root, int last)
        {
            int maxIndex = root;
            int maxValue = array[maxIndex];
            for(int index = root + 1; index < last; ++index)
                if(maxValue < array[index])
                {
                    maxIndex = index;
                    maxValue = array[index];
                }
            if (maxIndex != root)
                Swap(ref array[root], ref array[maxIndex]);
        }

        static void Heapify(int[] array, int size)
        {
            int root = (size >> 1) - 1;
            while(true)
            {
                SinkDown(array, root, size);
                if (root == 0)
                    break;
                --root;
            }
        }

        static void SinkDown(int[] array, int root, int size)
        {
            while (true)
            {
                int maxIndex = root;
                int leftIndex = (root << 1) + 1;
                int rightIndex = leftIndex + 1;
                if (leftIndex < size) { 
                    if (array[maxIndex] < array[leftIndex])
                        maxIndex = leftIndex;
                    if (rightIndex < size && array[maxIndex] < array[rightIndex])
                        maxIndex = rightIndex;
                }
                if (maxIndex == root)
                    break;
                Swap(ref array[root], ref array[maxIndex]);
                root = maxIndex;
            }
        }

        static int[] HeapSort(int[] array)
        {
            Heapify(array, array.Length);
            PrintArray("Heap", array);

            for (int size = array.Length; size > 0; --size)
            {
                SinkDown(array, 0, size);
                PrintArray("Sink", array);
                Swap(ref array[0], ref array[size - 1]);
                PrintArray("Swap last", array);
            }
            return array;
        }

        static void Main(string[] args)
        {
            int[] array = RandomizeArray(10);
            //int[] array = new int[15];
            //for(int index = 0; index < array.Length; ++index)
            //    array[index] = index + 1;
            PrintArray("Initial", array);
            HeapSort(array);
        }
    }
}
