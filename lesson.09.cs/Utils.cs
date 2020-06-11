using System;

namespace lesson._09.cs
{
    class Utils
    {
        static public void PrintArray(string text, int[] array)
        {
            Console.Write($"{text}:");
            for (int index = 0; index < array.Length; ++index)
                Console.Write($" {array[index]};");
            Console.WriteLine("");
        }

        static public int[] MakeIndexArray(int size)
        {
            int[] array = new int[size];
            for (int index = 0; index < size; ++index)
                array[index] = index;
            return array;
        }

        static void Swap(int[] array, int left, int right)
        {
            if (left == right)
                return;
            int t = array[left];
            array[left] = array[right];
            array[right] = t;
        }

        static public void Shuffle(int[] array)
        {
            if (array.Length < 2)
                return;

            Random rand = new Random();
            for (int i = 0; i < array.Length; ++i)
                Swap(array, i, rand.Next(i, array.Length));
        }

        static public int[] Sample(int[] array, int N)
        {
            int[] copy = new int[array.Length];
            Array.Copy(array, copy, array.Length);

            if (N > array.Length)
                N = array.Length;

            Random rand = new Random();
            for (int i = 0; i < N; ++i)
                Swap(copy, i, rand.Next(i, copy.Length));

            int[] sample = new int[N];
            Array.Copy(copy, sample, N);
            return sample;
        }

        static public bool IsOrdered(int[] array)
        {
            for (int index = 1; index < array.Length; ++index)
                if (array[index - 1] > array[index])
                    return false;
            return true;
        }

        static public bool IsHaveElement(int[] array, int element)
        {
            return Array.BinarySearch(array, element) >= 0;
        }

    }
}
