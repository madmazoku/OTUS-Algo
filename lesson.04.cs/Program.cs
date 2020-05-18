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
                Console.WriteLine("Добавление значений с одинаковым приоритетом");
                int[] expectArray = { 1, 2, 3 };

                PriorityQueue<int, int> priorityQueue = new PriorityQueue<int, int>();
                priorityQueue.enque(1, 1);
                priorityQueue.enque(1, 2);
                priorityQueue.enque(1, 3);
                int[] array = priorityQueue.Array;
                for (int index = 0; index < expectArray.Length; ++index)
                    if (expectArray[index] != array[index])
                    {
                        Console.WriteLine($"\tTest failed at index {index}");
                        return;
                    }
                Console.WriteLine("\tTest succeeded");
            }
            {
                Console.WriteLine("Добавление значений с разным приоритетом");
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
                        Console.WriteLine($"\tTest failed at index {index}");
                        return;
                    }
                Console.WriteLine("\tTest succeeded");
            }
            {
                Console.WriteLine("Извлечение значений");
                int[] expectArray = { 2, 1, 5, 4, 3 };

                PriorityQueue<int, int> priorityQueue = new PriorityQueue<int, int>();
                priorityQueue.enque(3, 1);
                priorityQueue.enque(4, 2);
                priorityQueue.enque(1, 3);
                priorityQueue.enque(2, 4);
                priorityQueue.enque(3, 5);
                for (int index = 0; index < expectArray.Length; ++index)
                {
                    if (expectArray[index] != priorityQueue.deque())
                    {
                        Console.WriteLine($"\tTest failed at deque {index}");
                        return;
                    }
                }
                Console.WriteLine("\tTest succeeded");
            }
        }
        static void Main(string[] args)
        {
            Console.WindowWidth = 152;
            Console.BufferWidth = 152;
            //TestArrays();
            TestPriorityQueue();
        }

    }
}
