using lesson._02.cs;

namespace lesson._03.cs
{
    class KnightTask : ITask
    {
        public string Name() { return "Ход коня"; }

        int where;
        ulong movesMask;

        public void Prepare(string[] data)
        {
            where = int.Parse(data[0]);
        }

        public bool Result(string[] expect)
        {
            ulong expectMovesMask = ulong.Parse(expect[1]);
            int expectMovesCount = int.Parse(expect[0]);
            int movesCount = Utils.CountBits(movesMask);
            return movesMask == expectMovesMask && movesCount == expectMovesCount;
        }

        public void Run()
        {
            movesMask = KnightMoves(where);
        }

        static ulong KnightMoves(int where)
        {
            ulong k = 1ul << where;
            ulong kL1 = k & 0xFEFEFEFEFEFEFEFE;
            ulong kL2 = k & 0xFCFCFCFCFCFCFCFC;
            ulong kR1 = k & 0x7F7F7F7F7F7F7F7F;
            ulong kR2 = k & 0x3F3F3F3F3F3F3F3F;
            ulong mask =
                (kL2 << 6) | (kL1 << 15) | (kR1 << 17) | (kR2 << 10) |
                (kL2 >> 10) | (kL1 >> 17) | (kR1 >> 15) | (kR2 >> 6);
            return mask;
        }

    }
}
