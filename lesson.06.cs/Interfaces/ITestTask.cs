using System.Threading;

namespace lesson._06.cs
{
    interface ITestTask
    {
        public string Name();

        public void Prepare(ITestCase testCase);
        public void Run(CancellationToken token);
        public bool Compare(ITestCase testCase);
    }
}
