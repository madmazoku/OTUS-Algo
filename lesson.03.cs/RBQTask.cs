using lesson._02.cs;

namespace lesson._03.cs
{
    class RBQTask : ITask
    {
        public string Name() { return "Дальнобойщики"; }

        ulong[] positions;
        ulong rookMoves;
        ulong bishopMoves;
        ulong queenMoves;

        public void Prepare(string[] data)
        {
            positions = Utils.ParseFEN(data[0]);
        }

        public bool Result(string[] expect)
        {
            ulong expectRookMoves = ulong.Parse(expect[0]);
            ulong expectBishopMoves = ulong.Parse(expect[1]);
            ulong expectQueenMoves = ulong.Parse(expect[2]);

            return expectRookMoves == rookMoves &&
                   expectBishopMoves == bishopMoves &&
                   expectQueenMoves == queenMoves;
        }

        public void Run()
        {
            rookMoves = RookMoves(positions);
            bishopMoves = BishopMoves(positions);
            queenMoves = QueenMoves(positions);
        }

        static ulong RookMoves(ulong[] positions)
        {
            ulong p = positions[3];

            ulong maskWhite = positions[0] | positions[1] | positions[2] | positions[3] | positions[4] | positions[5];
            ulong maskBlack = positions[6] | positions[7] | positions[8] | positions[9] | positions[10] | positions[11];
            ulong maskStop = maskWhite | maskBlack;
            ulong maskMove = ~maskStop;

            ulong maskBoardLeft = 0xFEFEFEFEFEFEFEFEul;
            ulong maskMoveLeft = maskBoardLeft & maskMove;

            ulong maskBoardRight = 0x7F7F7F7F7F7F7F7F;
            ulong maskMoveRight = maskBoardRight & maskMove;

            ulong pMLm = (p & maskBoardLeft) >> 1;
            ulong pMLF = pMLm;
            pMLm = (pMLm & maskMoveLeft) >> 1; pMLF |= pMLm;
            pMLm = (pMLm & maskMoveLeft) >> 1; pMLF |= pMLm;
            pMLm = (pMLm & maskMoveLeft) >> 1; pMLF |= pMLm;
            pMLm = (pMLm & maskMoveLeft) >> 1; pMLF |= pMLm;
            pMLm = (pMLm & maskMoveLeft) >> 1; pMLF |= pMLm;
            pMLm = (pMLm & maskMoveLeft) >> 1; pMLF |= pMLm;

            ulong pMRm = (p & maskBoardRight) << 1;
            ulong pMRF = pMRm;
            pMRm = (pMRm & maskMoveRight) << 1; pMRF |= pMRm;
            pMRm = (pMRm & maskMoveRight) << 1; pMRF |= pMRm;
            pMRm = (pMRm & maskMoveRight) << 1; pMRF |= pMRm;
            pMRm = (pMRm & maskMoveRight) << 1; pMRF |= pMRm;
            pMRm = (pMRm & maskMoveRight) << 1; pMRF |= pMRm;
            pMRm = (pMRm & maskMoveRight) << 1; pMRF |= pMRm;

            ulong pMUm = p << 8;
            ulong pMUF = pMUm;
            pMUm = (pMUm & maskMove) << 8; pMUF |= pMUm;
            pMUm = (pMUm & maskMove) << 8; pMUF |= pMUm;
            pMUm = (pMUm & maskMove) << 8; pMUF |= pMUm;
            pMUm = (pMUm & maskMove) << 8; pMUF |= pMUm;
            pMUm = (pMUm & maskMove) << 8; pMUF |= pMUm;
            pMUm = (pMUm & maskMove) << 8; pMUF |= pMUm;

            ulong pMDm = p >> 8;
            ulong pMDF = pMDm;
            pMDm = (pMDm & maskMove) >> 8; pMDF |= pMDm;
            pMDm = (pMDm & maskMove) >> 8; pMDF |= pMDm;
            pMDm = (pMDm & maskMove) >> 8; pMDF |= pMDm;
            pMDm = (pMDm & maskMove) >> 8; pMDF |= pMDm;
            pMDm = (pMDm & maskMove) >> 8; pMDF |= pMDm;
            pMDm = (pMDm & maskMove) >> 8; pMDF |= pMDm;

            ulong mask = (pMLF | pMRF | pMUF | pMDF) & ~maskWhite;

            return mask;
        }

        static ulong BishopMoves(ulong[] positions)
        {
            ulong p = positions[2];

            ulong maskWhite = positions[0] | positions[1] | positions[2] | positions[3] | positions[4] | positions[5];
            ulong maskBlack = positions[6] | positions[7] | positions[8] | positions[9] | positions[10] | positions[11];
            ulong maskStop = maskWhite | maskBlack;
            ulong maskMove = ~maskStop;

            ulong maskBoardLeft = 0xFEFEFEFEFEFEFEFEul;
            ulong maskMoveLeft = maskBoardLeft & maskMove;

            ulong maskBoardRight = 0x7F7F7F7F7F7F7F7F;
            ulong maskMoveRight = maskBoardRight & maskMove;

            ulong pMLUm = (p & maskBoardLeft) << 7;
            ulong pMLUF = pMLUm;
            pMLUm = (pMLUm & maskMoveLeft) << 7; pMLUF |= pMLUm;
            pMLUm = (pMLUm & maskMoveLeft) << 7; pMLUF |= pMLUm;
            pMLUm = (pMLUm & maskMoveLeft) << 7; pMLUF |= pMLUm;
            pMLUm = (pMLUm & maskMoveLeft) << 7; pMLUF |= pMLUm;
            pMLUm = (pMLUm & maskMoveLeft) << 7; pMLUF |= pMLUm;
            pMLUm = (pMLUm & maskMoveLeft) << 7; pMLUF |= pMLUm;

            ulong pMRUm = (p & maskBoardRight) << 9;
            ulong pMRUF = pMRUm;
            pMRUm = (pMRUm & maskMoveRight) << 9; pMRUF |= pMRUm;
            pMRUm = (pMRUm & maskMoveRight) << 9; pMRUF |= pMRUm;
            pMRUm = (pMRUm & maskMoveRight) << 9; pMRUF |= pMRUm;
            pMRUm = (pMRUm & maskMoveRight) << 9; pMRUF |= pMRUm;
            pMRUm = (pMRUm & maskMoveRight) << 9; pMRUF |= pMRUm;
            pMRUm = (pMRUm & maskMoveRight) << 9; pMRUF |= pMRUm;

            ulong pMLDm = (p & maskBoardLeft) >> 9;
            ulong pMLDF = pMLDm;
            pMLDm = (pMLDm & maskMoveLeft) >> 9; pMLDF |= pMLDm;
            pMLDm = (pMLDm & maskMoveLeft) >> 9; pMLDF |= pMLDm;
            pMLDm = (pMLDm & maskMoveLeft) >> 9; pMLDF |= pMLDm;
            pMLDm = (pMLDm & maskMoveLeft) >> 9; pMLDF |= pMLDm;
            pMLDm = (pMLDm & maskMoveLeft) >> 9; pMLDF |= pMLDm;
            pMLDm = (pMLDm & maskMoveLeft) >> 9; pMLDF |= pMLDm;

            ulong pMRDm = (p & maskBoardRight) >> 7;
            ulong pMRDF = pMRDm;
            pMRDm = (pMRDm & maskMoveRight) >> 7; pMRDF |= pMRDm;
            pMRDm = (pMRDm & maskMoveRight) >> 7; pMRDF |= pMRDm;
            pMRDm = (pMRDm & maskMoveRight) >> 7; pMRDF |= pMRDm;
            pMRDm = (pMRDm & maskMoveRight) >> 7; pMRDF |= pMRDm;
            pMRDm = (pMRDm & maskMoveRight) >> 7; pMRDF |= pMRDm;
            pMRDm = (pMRDm & maskMoveRight) >> 7; pMRDF |= pMRDm;

            ulong mask = (pMLUF | pMRUF | pMLDF | pMRDF) & ~maskWhite;

            return mask;
        }

        static ulong QueenMoves(ulong[] positions)
        {
            ulong p = positions[4];

            ulong maskWhite = positions[0] | positions[1] | positions[2] | positions[3] | positions[4] | positions[5];
            ulong maskBlack = positions[6] | positions[7] | positions[8] | positions[9] | positions[10] | positions[11];
            ulong maskStop = maskWhite | maskBlack;
            ulong maskMove = ~maskStop;

            ulong maskBoardLeft = 0xFEFEFEFEFEFEFEFEul;
            ulong maskMoveLeft = maskBoardLeft & maskMove;

            ulong maskBoardRight = 0x7F7F7F7F7F7F7F7F;
            ulong maskMoveRight = maskBoardRight & maskMove;

            ulong pMLm = (p & maskBoardLeft) >> 1;
            ulong pMLF = pMLm;
            pMLm = (pMLm & maskMoveLeft) >> 1; pMLF |= pMLm;
            pMLm = (pMLm & maskMoveLeft) >> 1; pMLF |= pMLm;
            pMLm = (pMLm & maskMoveLeft) >> 1; pMLF |= pMLm;
            pMLm = (pMLm & maskMoveLeft) >> 1; pMLF |= pMLm;
            pMLm = (pMLm & maskMoveLeft) >> 1; pMLF |= pMLm;
            pMLm = (pMLm & maskMoveLeft) >> 1; pMLF |= pMLm;

            ulong pMRm = (p & maskBoardRight) << 1;
            ulong pMRF = pMRm;
            pMRm = (pMRm & maskMoveRight) << 1; pMRF |= pMRm;
            pMRm = (pMRm & maskMoveRight) << 1; pMRF |= pMRm;
            pMRm = (pMRm & maskMoveRight) << 1; pMRF |= pMRm;
            pMRm = (pMRm & maskMoveRight) << 1; pMRF |= pMRm;
            pMRm = (pMRm & maskMoveRight) << 1; pMRF |= pMRm;
            pMRm = (pMRm & maskMoveRight) << 1; pMRF |= pMRm;

            ulong pMUm = p << 8;
            ulong pMUF = pMUm;
            pMUm = (pMUm & maskMove) << 8; pMUF |= pMUm;
            pMUm = (pMUm & maskMove) << 8; pMUF |= pMUm;
            pMUm = (pMUm & maskMove) << 8; pMUF |= pMUm;
            pMUm = (pMUm & maskMove) << 8; pMUF |= pMUm;
            pMUm = (pMUm & maskMove) << 8; pMUF |= pMUm;
            pMUm = (pMUm & maskMove) << 8; pMUF |= pMUm;

            ulong pMDm = p >> 8;
            ulong pMDF = pMDm;
            pMDm = (pMDm & maskMove) >> 8; pMDF |= pMDm;
            pMDm = (pMDm & maskMove) >> 8; pMDF |= pMDm;
            pMDm = (pMDm & maskMove) >> 8; pMDF |= pMDm;
            pMDm = (pMDm & maskMove) >> 8; pMDF |= pMDm;
            pMDm = (pMDm & maskMove) >> 8; pMDF |= pMDm;
            pMDm = (pMDm & maskMove) >> 8; pMDF |= pMDm;

            ulong maskRook = pMLF | pMRF | pMUF | pMDF;

            ulong pMLUm = (p & maskBoardLeft) << 7;
            ulong pMLUF = pMLUm;
            pMLUm = (pMLUm & maskMoveLeft) << 7; pMLUF |= pMLUm;
            pMLUm = (pMLUm & maskMoveLeft) << 7; pMLUF |= pMLUm;
            pMLUm = (pMLUm & maskMoveLeft) << 7; pMLUF |= pMLUm;
            pMLUm = (pMLUm & maskMoveLeft) << 7; pMLUF |= pMLUm;
            pMLUm = (pMLUm & maskMoveLeft) << 7; pMLUF |= pMLUm;
            pMLUm = (pMLUm & maskMoveLeft) << 7; pMLUF |= pMLUm;

            ulong pMRUm = (p & maskBoardRight) << 9;
            ulong pMRUF = pMRUm;
            pMRUm = (pMRUm & maskMoveRight) << 9; pMRUF |= pMRUm;
            pMRUm = (pMRUm & maskMoveRight) << 9; pMRUF |= pMRUm;
            pMRUm = (pMRUm & maskMoveRight) << 9; pMRUF |= pMRUm;
            pMRUm = (pMRUm & maskMoveRight) << 9; pMRUF |= pMRUm;
            pMRUm = (pMRUm & maskMoveRight) << 9; pMRUF |= pMRUm;
            pMRUm = (pMRUm & maskMoveRight) << 9; pMRUF |= pMRUm;

            ulong pMLDm = (p & maskBoardLeft) >> 9;
            ulong pMLDF = pMLDm;
            pMLDm = (pMLDm & maskMoveLeft) >> 9; pMLDF |= pMLDm;
            pMLDm = (pMLDm & maskMoveLeft) >> 9; pMLDF |= pMLDm;
            pMLDm = (pMLDm & maskMoveLeft) >> 9; pMLDF |= pMLDm;
            pMLDm = (pMLDm & maskMoveLeft) >> 9; pMLDF |= pMLDm;
            pMLDm = (pMLDm & maskMoveLeft) >> 9; pMLDF |= pMLDm;
            pMLDm = (pMLDm & maskMoveLeft) >> 9; pMLDF |= pMLDm;

            ulong pMRDm = (p & maskBoardRight) >> 7;
            ulong pMRDF = pMRDm;
            pMRDm = (pMRDm & maskMoveRight) >> 7; pMRDF |= pMRDm;
            pMRDm = (pMRDm & maskMoveRight) >> 7; pMRDF |= pMRDm;
            pMRDm = (pMRDm & maskMoveRight) >> 7; pMRDF |= pMRDm;
            pMRDm = (pMRDm & maskMoveRight) >> 7; pMRDF |= pMRDm;
            pMRDm = (pMRDm & maskMoveRight) >> 7; pMRDF |= pMRDm;
            pMRDm = (pMRDm & maskMoveRight) >> 7; pMRDF |= pMRDm;

            ulong maskBishop = pMLUF | pMRUF | pMLDF | pMRDF;

            ulong mask = (maskRook | maskBishop) & ~maskWhite;

            return mask;
        }

    }
}
