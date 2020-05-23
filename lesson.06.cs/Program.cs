using System;

namespace lesson._06.cs
{
    class Program
    {
        static void TestSort(string group, string path)
        {
            Tester tester = new Tester(group, new SortCase(), path, 6);
            tester.Add(new BubbleTask());
            tester.Add(new SelectionTask());
            tester.Add(new InsertionTask());
            tester.Add(new ShellTask(new BinarySequence()));
            tester.Add(new ShellTask(new KnuthSequence()));
            tester.Add(new ShellTask(new GonnetSequence()));
            tester.Add(new HeapTask());
            tester.Add(new QuickTask());
            tester.Add(new MergeTask());
            tester.RunTests();
            Console.WriteLine("");
        }

        static void Main(string[] args)
        {
            TestSort("Random", "0.random");
            TestSort("Digits", "1.digits");
            TestSort("Sorted", "2.sorted");
            TestSort("Reversed", "3.revers");
        }
    }
}
