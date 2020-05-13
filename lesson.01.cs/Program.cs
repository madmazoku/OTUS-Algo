using System;
using System.IO;

namespace lesson._01.cs
{
    class Program
    {
        static void ShowPainter()
        {
            Console.CursorVisible = false;

            Painter p = new Painter(50, 30);

            for (int i = 0; i < 500; i++)
                p.PutRandomNumbers();

            p.RandomFill();

            Console.ReadKey();
            Console.CursorVisible = true;
        }

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

        static void TestStringLen()
        {
            Console.WriteLine("Test Strings");
            ITask task = new StringLen();
            Tester tester = new Tester(task, $"{GetLessonsDataPath(1)}\\0.String");
            tester.RunTest();
        }

        static void TestTickets()
        {
            Console.WriteLine("Test Tickets");
            ITask task = new Tickets();
            Tester tester = new Tester(task, $"{GetLessonsDataPath(1)}\\1.Tickets");
            tester.RunTest();
        }

        static void ShowMagic()
        {
            Magic magic = new Magic();
            magic.SpellCraft();
        }

        static void Main(string[] args)
        {
            TestStringLen();
            Console.ReadKey();
            TestTickets();
            Console.ReadKey();
            ShowMagic();
        }
    }
}
