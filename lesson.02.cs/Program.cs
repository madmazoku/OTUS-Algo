using System;
using System.IO;
using System.Numerics;

namespace lesson._02.cs
{
    class Program
    {
        //static int Evklid(int a, int b)
        //{
        //    while (a != b)
        //        if (a > b)
        //            a = a - b;
        //        else
        //            b = b - a;
        //    return a;
        //}

        static long EvklidPlus(long a, long b)
        {
            while (a != 0 && b != 0)
                if (a > b)
                    a %= b;
                else
                    b %= a;
            return a != 0 ? a : b;
        }

        static long Shtein(long a, long b)
        {
            if ((a & 0x01) == 0 && (b & 0x01) == 0)
                return Shtein(a >> 1, b >> 1) << 1;
            else if ((a & 0x01) == 0)
                return Shtein(a >> 1, b);
            else if ((b & 0x01) == 0)
                return Shtein(a, b >> 1);
            else if (a > b)
                return Shtein((a - b) >> 1, b);
            else if (b < a)
                return Shtein(a, (b - a) >> 1);
            else
                return a;

        }

        //static double powMinus(double x, int y)
        //{
        //    double s = 1;
        //    for (int i = 0; i < y; i++)
        //        s *= x;
        //    return s;
        //}

        ////static double powMinusPlus(double x, int y)
        ////{
        ////    double s = 1;
        ////    while (y > 0 && (y & 0x01) == 0)
        ////    {
        ////        Console.WriteLine($"\tbefore; {y} : {s} : {x}");
        ////        s *= s;
        ////        y >>= 1;
        ////        Console.WriteLine($"\tafter;  {y} : {s} : {x}");
        ////    }
        ////    Console.WriteLine($"\tout;    {y} : {s} : {x}");
        ////    return ((y & 0x01) == 1 ? s : 1) * powMinus(x, y);
        ////}

        //static double pow2(double x)
        //{
        //    return x * x;
        //}
        //static double pow(double x, int y)
        //{
        //    if (y == 0)
        //        return 1;
        //    else if (y == 1)
        //        return x;
        //    else if ((y & 0x01) == 0)
        //        return pow2(pow(x, y >> 1));
        //    else
        //        return x * pow(x, y - 1);
        //}

        //static double powPlus(double x, int y)
        //{
        //    double s = 1;
        //    while (y > 1)
        //    {
        //        if ((y & 0x1) == 1)
        //            s *= x;
        //        x *= x;
        //        y >>= 1;
        //    }
        //    if (y > 0)
        //        s *= x;
        //    return s;
        //}

        //static bool IsPrimalMinus(int n)
        //{
        //    if ((n & 0x1) == 0)
        //        return n == 2;
        //    int sq = Convert.ToInt32(Math.Sqrt(n));
        //    for (int i = 3; j <= sq; j += 2)
        //        if (n % i == 0)
        //            return true;
        //    return false;
        //}
        //static int PrimalMinus(int n)
        //{
        //    if (n < 3)
        //        return 1;

        //    int cnt = 0;
        //    for (int i = 3; i <= n; ++i)
        //        if (IsPrimalMinus(i))
        //            ++cnt;

        //    return cnt;
        //}

        //foreach (d in primes while d <= s)

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
