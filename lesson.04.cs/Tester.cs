using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lesson._04.cs
{
    class Tester
    {
        private string group;
        private string path;
        private List<ITask> tasks = new List<ITask>();

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

        struct TestResult
        {
            bool success;
            double duration;
            bool exception;

            public TestResult(bool success, double duration, bool exception)
            {
                this.success = success;
                this.duration = duration;
                this.exception = exception;
            }

            public bool Success { get { return success; } }
            public double Duration { get { return duration; } }
            public bool Exception { get { return exception; } }
        }

        public Tester(string group, string path, long lesson)
        {
            this.group = group;
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

        public void Add(ITask task)
        {
            tasks.Add(task);
        }

        public void RunTests()
        {
            List<TestCase> testCases = LoadTestCases();
            Console.WriteLine(group);
            Console.Write($"{"",10}");
            foreach (ITask task in tasks)
                Console.Write($"| {task.Name(),25} ");
            Console.WriteLine("|");
            foreach (TestCase testCase in testCases)
            {
                Console.Write($"Test: #{testCase.TestCaseNumer,2} ");
                foreach (ITask task in tasks)
                {
                    TestResult testResult = RunTest(task, testCase);
                    Console.Write("|");
                    if (testResult.Exception)
                        Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write($" {testResult.Success,5} - {testResult.Duration,17:g8} ");
                    if (testResult.Exception)
                        Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine("|");
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

        private TestResult RunTest(ITask task, TestCase testCase)
        {
            Task<TestResult> asyncTask = Task.Run(
                () =>
                {
                    try
                    {
                        task.Prepare(testCase.Given);
                        long tries = 0;
                        Stopwatch sw = Stopwatch.StartNew();
                        do
                        {
                            task.Run();
                            ++tries;
                        } while (sw.Elapsed.TotalSeconds < 1.0);
                        sw.Stop();
                        return new TestResult(task.Result(testCase.Expect), sw.Elapsed.TotalSeconds / tries, false);
                    }
                    catch (Exception)
                    {
                        return new TestResult(false, 0, true);
                    }
                }
            );

            asyncTask.Wait();
            return asyncTask.Result;

            //if (asyncTask.Wait(TimeSpan.FromSeconds(10)))
            //    return asyncTask.Result;
            //else
            //    return new TestResult(false, 0, true);
        }
    }
}
