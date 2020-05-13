using lesson._02.cs;

namespace lesson._03.cs
{
    class KingTask : ITask
    {
        public string Name() { return "Ход короля"; }

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
            movesMask = KingMoves(where);
        }

        static ulong KingMoves(int where)
        {
            ulong k = 1ul << where;
            ulong kL = k & 0xFEFEFEFEFEFEFEFE;
            ulong kR = k & 0x7F7F7F7F7F7F7F7F;
            ulong mask = (kL << 7) | (k << 8) | (kR << 9) |
                (kL >> 1) | (kR << 1) |
                (kL >> 9) | (k >> 8) | (kR >> 7);
            return mask;
        }

    }
}
