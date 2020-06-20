namespace lesson._09.cs
{
    class Program
    {
        static void NodeTreeTest()
        {
            Tester tester = new Tester("Trees");
            tester.Add(new SimpleTree());
            tester.Add(new AVLTree());
            tester.Add(new DecartTree());
            tester.Add(new RandomTestCase(10));
            tester.Add(new OrderedTestCase(10, false));
            tester.Add(new OrderedTestCase(10, true));
            tester.Add(new RandomTestCase(100));
            tester.Add(new OrderedTestCase(100, false));
            tester.Add(new OrderedTestCase(100, true));
            tester.Add(new RandomTestCase(1000));
            tester.Add(new OrderedTestCase(1000, false));
            tester.Add(new OrderedTestCase(1000, true));
            tester.Add(new RandomTestCase(10000));
            tester.Add(new OrderedTestCase(10000, false));
            tester.Add(new OrderedTestCase(10000, true));
            tester.Add(new RandomTestCase(100000));
            tester.Add(new OrderedTestCase(100000, false));
            tester.Add(new OrderedTestCase(100000, true));
            tester.Add(new RandomTestCase(1000000));
            tester.Add(new OrderedTestCase(1000000, false));
            tester.Add(new OrderedTestCase(1000000, true));
            tester.Add(new RandomTestCase(10000000));
            tester.Add(new OrderedTestCase(10000000, false));
            tester.Add(new OrderedTestCase(10000000, true));
            tester.Add(new RandomTestCase(100000000));
            tester.RunTests();
        }

        static void Main(string[] args)
        {
            NodeTreeTest();
        }
    }
}
