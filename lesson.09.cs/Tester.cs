using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace lesson._09.cs
{
    class Tester
    {
        string group;
        bool shortTest;

        List<INodeTree> nodeTrees = new List<INodeTree>();
        List<ITestCase> testCases = new List<ITestCase>();

        struct TestResult
        {
            public ITestCase testCase;
            public INodeTree nodeTree;

            public bool timeout;
            public bool exception;

            public double durationTest;

            public bool successInsert;
            public bool successFind;
            public bool successRemove;

            public double durationInsert;
            public double durationFind;
            public double durationRemove;
        }

        public Tester(string group, bool shortTest = true)
        {
            this.group = group;
            this.shortTest = shortTest;
        }

        public void Add(INodeTree nodeTree)
        {
            nodeTrees.Add(nodeTree);
        }

        public void Add(ITestCase testCase)
        {
            testCases.Add(testCase);
        }

        public void RunTests()
        {
            Console.WriteLine($"{group}");
            foreach (ITestCase testCase in testCases)
            {
                Console.WriteLine($"\tTestCase: {testCase.Name()}");

                testCase.Prepare();

                CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(300));
                //CancellationTokenSource tokenSource = new CancellationTokenSource();

                List<Task<TestResult>> tasks = new List<Task<TestResult>>();
                foreach (INodeTree nodeTree in nodeTrees)
                    tasks.Add(Task<TestResult>.Run(() => RunTest(testCase, nodeTree, tokenSource.Token)));
                Task.WaitAll(tasks.ToArray());
                foreach (Task<TestResult> task in tasks)
                    PrintTestResult(task.Result);
            }
        }

        TestResult RunTest(ITestCase testCase, INodeTree nodeTree, CancellationToken token)
        {
            Stopwatch sw = Stopwatch.StartNew();
            TestResult testResult = new TestResult();
            testResult.testCase = testCase;
            testResult.nodeTree = nodeTree;
            try
            {
                INodeTree clonedNodeTree = nodeTree.Clone();
                (testResult.successInsert, testResult.durationInsert) = RunTestInsert(clonedNodeTree, testCase, token);
                (testResult.successFind, testResult.durationFind) = RunTestFind(clonedNodeTree, testCase, token);
                (testResult.successRemove, testResult.durationRemove) = RunTestRemove(clonedNodeTree, testCase, token);
            }
            catch (OperationCanceledException)
            {
                testResult.timeout = true;
            }
            catch (Exception)
            {
                testResult.exception = true;
            }
            sw.Stop();
            testResult.durationTest = sw.Elapsed.TotalSeconds;
            return testResult;
        }

        (bool, double) RunTestInsert(INodeTree nodeTree, ITestCase testCase, CancellationToken token)
        {
            int[] insertArray = testCase.GetInsertArray();
            Stopwatch sw = Stopwatch.StartNew();
            for (int index = 0; index < insertArray.Length; ++index)
            {
                token.ThrowIfCancellationRequested();
                nodeTree.Insert(insertArray[index]);
            }
            sw.Stop();
            double duration = sw.Elapsed.TotalSeconds;

            int[] checkArray = nodeTree.GetArray();
            bool success = Utils.IsOrdered(checkArray);
            if (success && shortTest)
            {
                success = true;
                for (int index = 0; index < insertArray.Length; ++index)
                {
                    token.ThrowIfCancellationRequested();
                    if (!Utils.IsHaveElement(checkArray, insertArray[index]))
                    {
                        success = false;
                        break;
                    }
                }
            }
            return (success, duration);
        }

        (bool, double) RunTestFind(INodeTree nodeTree, ITestCase testCase, CancellationToken token)
        {
            int[] findArray = testCase.GetFindArray();
            bool[] findResults = new bool[findArray.Length];
            Stopwatch sw = Stopwatch.StartNew();
            for (int index = 0; index < findArray.Length; ++index)
            {
                token.ThrowIfCancellationRequested();
                findResults[index] = nodeTree.Find(findArray[index]);
            }
            sw.Stop();
            double duration = sw.Elapsed.TotalSeconds;

            int[] checkArray = nodeTree.GetArray();
            bool success = Utils.IsOrdered(checkArray);
            if (success && shortTest)
            {
                success = true;
                for (int index = 0; index < findArray.Length; ++index)
                {
                    token.ThrowIfCancellationRequested();
                    if (Utils.IsHaveElement(checkArray, findArray[index]) != findResults[index])
                    {
                        success = false;
                        break;
                    }
                }
            }
            return (success, duration);
        }

        (bool, double) RunTestRemove(INodeTree nodeTree, ITestCase testCase, CancellationToken token)
        {
            return (false, 0);

            int[] removeArray = testCase.GetRemoveArray();
            Stopwatch sw = Stopwatch.StartNew();
            for (int index = 0; index < removeArray.Length; ++index)
            {
                token.ThrowIfCancellationRequested();
                nodeTree.Remove(removeArray[index]);
            }
            sw.Stop();
            double duration = sw.Elapsed.TotalSeconds;

            int[] checkArray = nodeTree.GetArray();
            bool success = Utils.IsOrdered(checkArray);
            if (success && shortTest)
            {
                success = true;
                for (int index = 0; index < removeArray.Length; ++index)
                {
                    token.ThrowIfCancellationRequested();
                    if (Utils.IsHaveElement(checkArray, removeArray[index]))
                    {
                        success = false;
                        break;
                    }
                }
            }
            return (success, duration);
        }

        void PrintTestResult(TestResult testResult)
        {
            Console.Write($"\t\t{testResult.nodeTree.Name(),20} [{testResult.durationTest,10:g8}]:");
            if (testResult.timeout)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("Timeout");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else if (testResult.exception)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("Exception");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                PrintSubTestResult("insert", testResult.successInsert, testResult.durationInsert);
                PrintSubTestResult("find", testResult.successFind, testResult.durationFind);
                PrintSubTestResult("remove", testResult.successRemove, testResult.durationRemove);
            }

            Console.WriteLine("");

        }

        void PrintSubTestResult(string text, bool success, double duration)
        {
            Console.Write($" {text}: ");
            if (!success)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write($"{duration,10:g8}");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
                Console.Write($"{duration,10:g8}");
            Console.Write(";");
        }
    }
}
