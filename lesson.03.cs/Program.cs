using lesson._02.cs;

namespace lesson._03.cs
{
    class Program
    {
        static void TestKing()
        {
            Tester tester = new Tester(3, "1.Bitboard - Король");
            KingTask task = new KingTask();
            tester.RunTests(task);
        }

        static void Main(string[] args)
        {
            TestKing();
        }
    }
}
