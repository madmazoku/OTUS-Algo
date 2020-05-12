using System;
using System.IO;
using System.Numerics;

namespace lesson._02.cs
{
    class Program
    {
        static string GetLessonsDataPath(int lesson)
        {
            string lessonDir = $"//lesson.{lesson:d2}.data";
            DirectoryInfo path = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (path != null)
            {
                if (Directory.Exists(path.ToString() + lessonDir))
                {
                    return path.ToString() + lessonDir;
                }
                path = path.Parent;
            }
            throw new Exception($"{lessonDir} directory not found through the parents of current directory");
        }

        static void TestNOD()
        {
            Tester tester = new Tester("Тестирование поиска Наибольшего Общего Делителя", $"{GetLessonsDataPath(2)}\\2.GCD");
            tester.Add(new GCDDeductionTask());
            tester.Add(new GCDRemainderTask());
            tester.Add(new GCDSteinRecursiveTask());
            tester.Add(new GCDSteinIterativeTask());
            tester.RunTests();
        }

        static void TestPOW()
        {
            Tester tester = new Tester("Тестирование возведения в степень", $"{GetLessonsDataPath(2)}\\3.Power");
            tester.Add(new POWIterativeTask());
            tester.Add(new POWBinMulIterateTask());
            tester.Add(new POWBinRecursiveTask());
            tester.Add(new POWBinIterativeTask());
            tester.RunTests();
        }

        static void TestPrimes()
        {
            Tester tester = new Tester("Тестирование простых чисел", $"{GetLessonsDataPath(2)}\\5.Primes");
            tester.Add(new PrimesNativeTask());
            tester.Add(new PrimesListTask());
            tester.Add(new PrimesEratostheneTask());
            tester.Add(new PrimesEratostheneBitsTask());
            tester.Add(new PrimesEratostheneFastTask());
            tester.RunTests();
        }

        static void TestFibonacci()
        {
            Tester tester = new Tester("Тестирование чисел Фибоначчи", $"{GetLessonsDataPath(2)}\\4.Fibo");
            tester.Add(new FibonacciRecursiveTask());
            tester.Add(new FibonacciIterativeTask());
            tester.Add(new FibonacciGoldenTask());
            tester.Add(new FibonacciMatrixTask());
            tester.Add(new FibonacciMatrixParallelTask());
            tester.RunTests();
        }

        static void Main(string[] args)
        {
            Console.WindowWidth = 152;
            Console.BufferWidth = 152;
            TestNOD();
            TestPOW();
            TestPrimes();
            TestFibonacci();
        }
    }
}
