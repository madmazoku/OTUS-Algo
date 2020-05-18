using System;

namespace lesson._04.cs
{
    class Program
    {

        static void TestArrays()
        {
            Tester tester = new Tester("Тестирование динамических массивов", "1.Arrays", 4);
            tester.Add(new ArrayTask<SingleArray<int>>("Single"));
            tester.Add(new ArrayTask<VectorArray<int>>("Vector"));
            tester.Add(new ArrayTask<FactorArray<int>>("Factor"));
            tester.Add(new ArrayTask<MatrixArray<int>>("Matrix"));
            tester.Add(new ArrayTask<ListArray<int>>("List"));
            tester.RunTests();
        }
        static void TestPriorityQueue()
        {
            Console.WriteLine("Тестирование приоритетной очереди");
            {
                Console.WriteLine("\tДобавление значений с одинаковым приоритетом");
                int[] expectArray = { 1, 2, 3 };

                PriorityQueue<int, int> priorityQueue = new PriorityQueue<int, int>();
                priorityQueue.enque(1, 1);
                priorityQueue.enque(1, 2);
                priorityQueue.enque(1, 3);
                int[] array = priorityQueue.Array;
                for (int index = 0; index < expectArray.Length; ++index)
                    if (expectArray[index] != array[index])
                    {
                        Console.WriteLine($"\t\tTest failed at index {index}");
                        return;
                    }
                Console.WriteLine("\t\tTest succeeded");
            }
            {
                Console.WriteLine("\tДобавление значений с разным приоритетом");
                int[] expectArray = { 2, 1, 5, 4, 3 };

                PriorityQueue<int, int> priorityQueue = new PriorityQueue<int, int>();
                priorityQueue.enque(3, 1);
                priorityQueue.enque(4, 2);
                priorityQueue.enque(1, 3);
                priorityQueue.enque(2, 4);
                priorityQueue.enque(3, 5);
                int[] array = priorityQueue.Array;
                for (int index = 0; index < expectArray.Length; ++index)
                    if (expectArray[index] != array[index])
                    {
                        Console.WriteLine($"\t\tTest failed at index {index}");
                        return;
                    }
                Console.WriteLine("\t\tTest succeeded");
            }
            {
                Console.WriteLine("\tУдаление значений");
                int[] expectArray = { 2, 1, 5, 4, 3 };

                PriorityQueue<int, int> priorityQueue = new PriorityQueue<int, int>();
                priorityQueue.enque(3, 1);
                priorityQueue.enque(4, 2);
                priorityQueue.enque(1, 3);
                priorityQueue.enque(2, 4);
                priorityQueue.enque(3, 5);
                for (int index = 0; index < expectArray.Length; ++index)
                    if (expectArray[index] != priorityQueue.deque())
                    {
                        Console.WriteLine($"\t\tTest failed at deque {index}");
                        return;
                    }
                Console.WriteLine("\t\tTest succeeded");
            }
            Console.WriteLine("");
        }
        static void TestSparseArray()
        {
            Console.WriteLine("Тестирование разряжённого массива");
            {
                Console.WriteLine("\tДобавление значений");

                SparseArray<int> sparseArray = new SparseArray<int>();
                int[] expectArray = { 0, 1, 0, 2, 3, 0, 4 };

                sparseArray.Add(1, 1);
                sparseArray.Add(4, 4);
                sparseArray.Add(3, 3);
                sparseArray.Add(2, 3);

                for (int index = 0; index < expectArray.Length; ++index)
                    if (expectArray[index] != sparseArray.Get(index))
                    {
                        Console.WriteLine($"\t\tTest failed at index {index}");
                        return;
                    }
                Console.WriteLine("\t\tTest succeeded");
            }
            {
                Console.WriteLine("\tУстановка значений");

                SparseArray<int> sparseArray = new SparseArray<int>();
                int[] expectArray = { 0, 1, 0, 2, 3, 0, 4 };

                sparseArray.Set(1, 1);
                sparseArray.Set(4, 6);
                sparseArray.Set(5, 4);
                sparseArray.Set(2, 3);
                sparseArray.Set(3, 4);

                for (int index = 0; index < expectArray.Length; ++index)
                    if (expectArray[index] != sparseArray.Get(index))
                    {
                        Console.WriteLine($"\t\tTest failed at index {index}");
                        return;
                    }
                Console.WriteLine("\t\tTest succeeded");
            }
            {
                Console.WriteLine("\tУдаление значений");

                SparseArray<int> sparseArray = new SparseArray<int>();
                int[] expectRemoveArray = { 2, 3, 0 };
                int[] expectArray = { 0, 1, 0, 4 };

                sparseArray.Set(1, 1);
                sparseArray.Set(4, 6);
                sparseArray.Set(3, 4);
                sparseArray.Set(2, 3);
                for (int index = 0; index < expectRemoveArray.Length; ++index)
                    if (expectRemoveArray[index] != sparseArray.Remove(3))
                    {
                        Console.WriteLine($"\t\tTest failed at remove 3 try {index}");
                        return;
                    }
                for (int index = 0; index < expectArray.Length; ++index)
                    if (expectArray[index] != sparseArray.Get(index))
                    {
                        Console.WriteLine($"\t\tTest failed at index {index}");
                        return;
                    }
                Console.WriteLine("\t\tTest succeeded");
            }
            Console.WriteLine("");
        }
        static void Main(string[] args)
        {
            Console.WindowWidth = 152;
            Console.BufferWidth = 152;
            TestArrays();
            TestPriorityQueue();
            TestSparseArray();
        }

    }
}
