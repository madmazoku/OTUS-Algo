using System;
using System.Diagnostics;
using System.Numerics;

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
        static void Main(string[] args)
        {
            Console.WindowWidth = 152;
            Console.BufferWidth = 152;
            TestArrays();
        }

    }
}
