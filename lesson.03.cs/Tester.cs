using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lesson._02.cs
{
    class Tester
    {
        private string path;

        struct TestCase
        {
            int testCaseNumber;
            string[] given;
            string[] expect;

            public TestCase(int testCaseNumber, string[] given, string[] expect)
            {
                this.testCaseNumber = testCaseNumber;
                this.given = given;
                this.expect = expect;
            }

            public int TestCaseNumer { get { return testCaseNumber; } }
            public string[] Given { get { return given; } }
            public string[] Expect { get { return expect; } }
        };

        public Tester(long lesson, string path)
        {
            this.path = GetTestPath(lesson, path);
        }

        private string GetTestPath(long lesson, string testDir)
        {
            string lessonTestDir = $"//lesson.{lesson:d2}.data//{testDir}";
            DirectoryInfo path = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (path != null)
            {
                if (Directory.Exists(path.ToString() + lessonTestDir))
                {
                    return path.ToString() + lessonTestDir;
                }
                path = path.Parent;
            }
            throw new Exception($"{lessonTestDir} directory not found through the parents of current directory");
        }

        public void RunTests(ITask task)
        {
            List<TestCase> testCases = LoadTestCases();
            Console.WriteLine(task.Name());
            foreach (TestCase testCase in testCases)
            {
                task.Prepare(testCase.Given);
                task.Run();
                bool success = task.Result(testCase.Expect);
                Console.WriteLine($"Test: #{testCase.TestCaseNumer,2}: {success}");
            }
            Console.WriteLine("");
        }

        private List<TestCase> LoadTestCases()
        {
            List<TestCase> test_cases = new List<TestCase>();
            int test_case_number = 0;
            while (true)
            {
                string inFile = $"{path}\\test.{test_case_number}.in";
                string outFile = $"{path}\\test.{test_case_number}.out";
                if (!File.Exists(inFile) || !File.Exists(outFile))
                    break;

                string[] given = File.ReadAllLines(inFile).Select(x => x.Trim()).ToArray();
                string[] expect = File.ReadAllLines(outFile).Select(x => x.Trim()).ToArray();
                test_cases.Add(new TestCase(test_case_number, given, expect));
                ++test_case_number;
            }
            return test_cases;
        }

    }
}
