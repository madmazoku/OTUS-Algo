using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace lesson._06.cs
{
    class Tester
    {
        private string group;
        private string path;
        private List<ITestTask> tasks = new List<ITestTask>();
        private ITestCase testCase;

        public int Count { get { return tasks.Count; } }

        struct RawTestCase
        {
            int testCaseNumber;
            string[] given;
            string[] expect;

            public RawTestCase(int testCaseNumber, string[] given, string[] expect)
            {
                this.testCaseNumber = testCaseNumber;
                this.given = given;
                this.expect = expect;
            }

            public int TestCaseNumer { get { return testCaseNumber; } }
            public string[] Given { get { return given; } }
            public string[] Expect { get { return expect; } }
        };

        public Tester(string group, ITestCase testCase, string path, long lesson)
        {
            this.group = group;
            this.testCase = testCase;
            this.path = Utils.GetTestPath(lesson, path);
        }

        public void Add(ITestTask task)
        {
            tasks.Add(task);
        }

        public void RunTests()
        {
            List<RawTestCase> testCases = LoadTestCases();

            int width = 10 + 28 * tasks.Count + 2;
            if (width < Console.WindowWidth)
                Console.BufferWidth = Console.WindowWidth = width;
            else
                Console.WindowWidth = Console.BufferWidth = width;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine(group);
            Console.Write($"{"",10}");
            foreach (ITestTask task in tasks)
                Console.Write($"| {task.Name(),25} ");
            Console.WriteLine("|");
            foreach (RawTestCase rawTestCase in testCases)
            {
                testCase.Prepare(rawTestCase.Given, rawTestCase.Expect);

                Console.Write($"Test: #{rawTestCase.TestCaseNumer,2} ");

                List<(ITestTask, Task<(double, bool, bool)>)> runTests = new List<(ITestTask, Task<(double, bool, bool)>)>();
                CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                //CancellationTokenSource tokenSource = new CancellationTokenSource();
                foreach (ITestTask task in tasks)
                    runTests.Add((task, RunTask(task, testCase, tokenSource.Token)));

                foreach ((ITestTask task, Task<(double, bool, bool)> asyncTask) in runTests)
                {
                    asyncTask.Wait();
                    (double duration, bool exception, bool canceled) = asyncTask.Result;
                    bool success = !exception && task.Compare(testCase);

                    Console.Write("|");
                    if (exception)
                    {
                        Console.BackgroundColor = canceled ? ConsoleColor.Yellow : ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.Write($" {success,5} - {duration,17:g8} ");
                    if (exception)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("");
        }

        private List<RawTestCase> LoadTestCases()
        {
            List<RawTestCase> test_cases = new List<RawTestCase>();
            int testCaseNumber = 0;
            while (true)
            {
                string inFile = $"{path}\\test.{testCaseNumber}.in";
                string outFile = $"{path}\\test.{testCaseNumber}.out";
                if (!File.Exists(inFile) || !File.Exists(outFile))
                    break;

                string[] given = File.ReadAllLines(inFile).Select(x => x.Trim()).ToArray();
                string[] expect = File.ReadAllLines(outFile).Select(x => x.Trim()).ToArray();
                test_cases.Add(new RawTestCase(testCaseNumber, given, expect));
                ++testCaseNumber;
            }
            return test_cases;
        }

        private Task<(double, bool, bool)> RunTask(ITestTask task, ITestCase testCase, CancellationToken token)
        {
            return Task.Run(() =>
            {
                try
                {
                    long tries = 0;
                    double duration = 0;
                    do
                    {
                        task.Prepare(testCase);
                        Stopwatch sw = Stopwatch.StartNew();
                        task.Run(token);
                        sw.Stop();
                        duration += sw.Elapsed.TotalSeconds;
                        ++tries;
                    } while (duration < 1.0);
                    return (duration / tries, false, false);
                }
                catch (OperationCanceledException)
                {
                    return (0, true, true);
                }
                catch (Exception)
                {
                    return (0, true, false);
                }
            }, token);
        }
    }
}
