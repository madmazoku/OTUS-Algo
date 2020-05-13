using System;
using System.IO;

namespace lesson._01.cs
{
    class Tester
    {
        ITask task;
        string path;

        public Tester(ITask task, string path)
        {
            this.task = task;
            this.path = path;
        }

        public void RunTest()
        {
            int nr = 0;
            while (true)
            {
                string inFile = $"{path}\\test.{nr}.in";
                string outFile = $"{path}\\test.{nr}.out";
                if (!File.Exists(inFile) || !File.Exists(outFile))
                    break;
                Console.WriteLine($"Test: #{nr} - " + RunTest(inFile, outFile));
                nr++;
            }
        }

        bool RunTest(string inFile, string outFile)
        {
            try
            {
                string[] data = File.ReadAllLines(inFile);
                string expect = File.ReadAllText(outFile).Trim();
                string actual = task.Run(data);
                return actual == expect;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
