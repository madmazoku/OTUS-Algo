using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace lesson._08.cs
{
    class Program
    {
        static void PrintArray(string name, int[] array)
        {
            Console.WriteLine($"{name,20}: ");
            for (int index = 0; index < array.Length; ++index)
                Console.Write($" [{index,2}] {array[index]};");
            Console.WriteLine("");
        }

        static int[] RandomArray(int N, int R)
        {
            int[] array = new int[N];
            Random rand = new Random();
            for (int index = 0; index < N; ++index)
                array[index] = (int)(R * rand.NextDouble());
            return array;
        }

        static int[] CopyArray(int[] array)
        {
            int[] aux = new int[array.Length];
            Array.Copy(array, aux, array.Length);
            return aux;
        }

        static void CountSort(int[] aux, int[] array, int R, Func<int, int> Indexer)
        {
            int[] counts = new int[R];
            for (int index = 0; index < array.Length; ++index)
                ++counts[Indexer(array[index])];

            for (int index = 1; index < R; ++index)
                if (counts[index] == array.Length)
                    return;
                else
                    counts[index] += counts[index - 1];

            Array.Copy(array, aux, array.Length);
            for (int index = array.Length - 1; index >= 0; --index)
                array[--counts[Indexer(aux[index])]] = aux[index];
        }

        static void RadixSort(int[] array, int R)
        {
            int max = array[1];
            for (int index = 1; index < array.Length; ++index)
                if (max < array[index])
                    max = array[index];

            int[] aux = new int[array.Length];

            int d = 1;
            while (d <= max) {
                CountSort(aux, array, R, x => (x / d) % R);
                d *= R;
            }
        }

        class Node
        {
            public int value;
            public Node next;

            public Node(int value, Node next)
            {
                this.value = value;
                this.next = next;
            }
        }

        static void BucketSort(int[] array, int B)
        {
            int max = array[1];
            for (int index = 1; index < array.Length; ++index)
                if (max < array[index])
                    max = array[index];

            Node[] buckets = new Node[B];

            for (int index = 0; index < array.Length; ++index)
            {
                int value = array[index];
                int bucketIndex = array[index] * B / (max + 1);
                Node node = buckets[bucketIndex];
                if (node == null)
                    buckets[bucketIndex] = new Node(value, null);
                else
                {
                    Node prevNode = null;
                    while(node != null && node.value < value)
                    {
                        prevNode = node;
                        node = node.next;
                    }
                    if (prevNode == null)
                        buckets[bucketIndex] = new Node(value, node);
                    else
                        prevNode.next = new Node(value, node);
                }
            }

            for(int index = 0, bucketIndex = 0; bucketIndex < B; ++bucketIndex)
                for(Node node = buckets[bucketIndex]; node != null; node = node.next)
                    array[index++] = node.value;
        }

        static int GetPivot(int[] aux, int[] array, int left, int right)
        {
            if (right - left <= 5)
            {
                Array.Sort(array, left, right - left);
                return array[left + ((right - left) >> 1)];
            }


            int pivotIndex = 0;
            for (int index = left; index + 5 < right; index += 5)
            {
                Array.Sort(array, index, 5);
                aux[pivotIndex++] = array[index + 3];
            }
            Array.Sort(aux, 0, pivotIndex);
            return aux[pivotIndex >> 1];
        }

        static int Part(int[] aux, int[] array, int left, int right, int pivot)
        {
            int pLeft = left;
            int pRight = right; 
            for (int index = left; index < right; ++index)
                if (array[index] <= pivot)
                    aux[pLeft++] = array[index];
                else
                    aux[--pRight] = array[index];
            Array.Copy(aux, left, array, left, right - left);

            return pRight;
        }

        static int Percentile(int[] array, int N)
        {
            int[] aux = new int[array.Length];

            int left = 0;
            int right = array.Length;
            while (N > 0 && N != right - left)
            {
                int pivot = GetPivot(aux, array, left, right);

                int mid = Part(aux, array, left, right, pivot);

                if (mid - left > N)
                    right = mid;
                else
                {
                    N -= mid - left;
                    left = mid;
                }
            }

            if (N == 0)
            {
                int min = array[left];
                for (int index = left + 1; index < right; ++index)
                    if (min > array[index])
                        min = array[index];

                return min;
            } else
            {
                int max = array[left];
                for (int index = left + 1; index < right; ++index)
                    if (max < array[index])
                        max = array[index];

                return max;
            }
        }

        static void Main(string[] args)
        {
            //int[] array = RandomArray(21, 10);
            int[] array = { 5, 0, 2, 5, 4, 3, 8, 8, 2, 3, 1, 6, 7, 1, 6, 0, 6, 0, 3, 3, 9 };
            int[] arrayCopy = CopyArray(array);
            int N = 6;
            PrintArray("original", array);
            Array.Sort(arrayCopy);
            PrintArray("sorted", arrayCopy);
            int percentile = Percentile(array, N);
            Console.WriteLine($"     percentile {N} is {percentile}");
            Console.WriteLine($"real percentile {N} is {arrayCopy[N]}");
        }
    }
}
