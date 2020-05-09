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
        private string group;
        private string path;
        private List<ITask> tasks = new List<ITask>();

        struct TestCase
        {
            int testCaseNumber;
            string[] given;
            string expect;

            public TestCase(int testCaseNumber, string[] given, string expect)
            {
                this.testCaseNumber = testCaseNumber;
                this.given = given;
                this.expect = expect;
            }

            public int TestCaseNumer { get { return testCaseNumber; } }
            public string[] Given { get { return given; } }
            public string Expect { get { return expect; } }
        };

        struct TestResult
        {
            bool success;
            double duration;

            public TestResult(bool success, double duration)
            {
                this.success = success;
                this.duration = duration;
            }

            public bool Success { get { return success; } }
            public double Duration { get { return duration; } }
        }

        public Tester(string group, string path)
        {
            this.group = group;
            this.path = path;
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
                    Console.Write($"| {testResult.Success,5} - {testResult.Duration,17:g8} ");
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
                string expect = File.ReadAllText(outFile).Trim();
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
                        return new TestResult(task.Result(testCase.Expect), sw.Elapsed.TotalSeconds / tries);
                    }
                    catch (Exception)
                    {
                        return new TestResult(false, 0);
                    }
                }
            );

            //asyncTask.Wait();
            //return asyncTask.Result;

            if (asyncTask.Wait(TimeSpan.FromSeconds(10)))
                return asyncTask.Result;
            else
                return new TestResult(false, 0);
        }
    }
}
