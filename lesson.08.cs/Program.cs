using System;

namespace lesson._08.cs
{
    class Program
    {
        /***********************************************************
         * Out of Homework Code                                    *
         ***********************************************************/
        /*
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
            while (d <= max)
            {
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
                    while (node != null && node.value < value)
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

            for (int index = 0, bucketIndex = 0; bucketIndex < B; ++bucketIndex)
                for (Node node = buckets[bucketIndex]; node != null; node = node.next)
                    array[index++] = node.value;
        }

        static int GetPivot(int[] aux, int[] array, int left, int right)
        {
            if (right - left <= 5)
            {
                Array.Copy(array, left, aux, left, right - left);
                Array.Sort(aux, left, right - left);
                return aux[left + ((right - left) >> 1)];
            }

            int pivotIndex = 0;
            for (int index = left; index + 5 < right; index += 5)
            {
                Array.Sort(array, index, 5);
                aux[pivotIndex++] = array[index + 2];
            }
            Array.Sort(aux, 0, pivotIndex);
            return aux[pivotIndex >> 1];
        }

        static (int, int) Part(int[] aux, int[] array, int left, int right, int pivot)
        {
            int pLeft = left;
            int pRight = right;
            for (int index = left; index < right; ++index)
                if (array[index] < pivot)
                    aux[pLeft++] = array[index];
                else if (array[index] > pivot)
                    aux[--pRight] = array[index];
            for (int index = pLeft; index < pRight; ++index)
                aux[index] = pivot;
            Array.Copy(aux, left, array, left, right - left);

            return (pLeft, pRight);
        }

        static int Percentile(int[] array, int N)
        {
            int[] aux = new int[array.Length];

            int left = 0;
            int right = array.Length;
            while (right - left > 1)
            {
                int pivot = GetPivot(aux, array, left, right);

                (int pLeft, int pRight) = Part(aux, array, left, right, pivot);

                if (pLeft - left > N)
                    right = pLeft;
                else if (pRight - left > N)
                    return pivot;
                else
                {
                    N -= pRight - left;
                    left = pRight;
                }
            }

            return array[left];
        }
        */

        static void TestFileSorts()
        {
            Tester tester = new Tester("File Sorts", 8);

            tester.Add(new MergeFileSort(new MergeMASort(64, new QuickMASort()), 10));
            tester.Add(new MergeFileSort(new MergeMASort(64, new QuickMASort()), 100));
            tester.Add(new MergeFileSort(new MergeMASort(64, new QuickMASort()), 1_000));
            tester.Add(new MergeFileSort(new QuickMASort(), 10));
            tester.Add(new MergeFileSort(new QuickMASort(), 100));
            tester.Add(new MergeFileSort(new QuickMASort(), 1_000));
            tester.Add(new MMFFileSort(new QuickMASort()));
            tester.Add(new MMFFileSort(new HeapMASort()));

            tester.Add(new BucketFileSort(10));
            tester.Add(new BucketFileSort(100));
            tester.Add(new BucketFileSort(1_000));
            tester.Add(new CounterFileSort());
            tester.Add(new RadixFileSort());

            long[] arraySizes = { 10_000, 100_000, 1_000_000, 10_000_000, 100_000_000, 1_000_000_000 };

            tester.RunTests(arraySizes);
        }

        static void Main(string[] args)
        {
            TestFileSorts();

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
