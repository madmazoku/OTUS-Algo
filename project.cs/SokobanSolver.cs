using System;
using System.Collections.Generic;

namespace project.cs
{

    class SokobanSolver
    {
        SokobanSolverMap map;
        SokobanSolverExplorer explorer;

        ArrayComparer<ushort> arrayComparer;

        // child state reference to it's parent
        Dictionary<ushort[], ushort[]> moves;
        // yet to process states;
        Queue<ushort[]> states;

        public SokobanSolver(string path)
        {
            map = new SokobanSolverMap(path);

            arrayComparer = new ArrayComparer<ushort>();
            moves = new Dictionary<ushort[], ushort[]>(arrayComparer);
            states = new Queue<ushort[]>();

            explorer = new SokobanSolverExplorer(map);
        }

        static int cnt = 0;

        public void Run()
        {
            ushort[] victoryState = new ushort[map.boxesCount + 1];
            Array.Copy(map.targetXYs, victoryState, map.boxesCount);

            ushort[] startState = new ushort[map.boxesCount + 1];
            Array.Copy(map.boxXYs, startState, map.boxesCount);
            startState[map.boxesCount] = map.playerXY;

            states.Enqueue(startState);

            SokobanStepper stepper = new SokobanStepper(map);
            int countTries = 0;
            bool isVictory = false;
            while (states.Count > 0)
            {
                ++countTries;

                if (++cnt == 100000)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine($"states to explore: {states.Count}".PadRight(Console.BufferWidth));
                    cnt = 0;
                }

                ushort[] state = states.Dequeue();

                victoryState[map.boxesCount] = state[map.boxesCount];
                if (arrayComparer.Equals(victoryState, state))
                {
                    isVictory = true;
                    break;
                }

                stepper.Next(state);

                stepper.Queue(moves, states);
            }

            if (isVictory)
            {
                Console.Clear();
                Console.WriteLine($"Victory found in {countTries} tries");

                Stack<ushort[]> solutionBoxStack = new Stack<ushort[]>();
                ushort[] state = new ushort[map.boxesCount + 1];
                Array.Copy(victoryState, state, map.boxesCount + 1);

                while (!arrayComparer.Equals(startState, state))
                {
                    solutionBoxStack.Push(state);
                    if (!moves.TryGetValue(state, out state))
                        throw new Exception("broken solution");
                }
                solutionBoxStack.Push(startState);

                ushort[][] solutionBox = solutionBoxStack.ToArray();
                ushort[][] solutionPath = new ushort[solutionBox.Length][];
                for (int i = 0; i < solutionBox.Length - 1; ++i)
                    solutionPath[i] = BuildPath(solutionBox[i], solutionBox[i + 1]);
                solutionPath[solutionBox.Length - 1] = new ushort[] { solutionBox[solutionBox.Length - 1][map.boxesCount] };

                int countMoves = -1;
                for (int i = 0; i < solutionPath.Length; ++i)
                    countMoves += solutionPath[i].Length;

                Console.SetCursorPosition(0, 2 + map.height);
                Console.WriteLine("Use left and right arrow keys to explore solution");

                int posMove = 0;
                int posBox = 0;
                int posPlayer = 0;
                while (true)
                {
                    Console.SetCursorPosition(0, 1);
                    Console.Write($"At {posMove} move from {countMoves}".PadRight(Console.BufferWidth));

                    Render(solutionBox[posBox], solutionPath[posBox][posPlayer]);

                    ConsoleKeyInfo cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Escape)
                        break;

                    switch (cki.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            --posMove;
                            if (posPlayer > 0)
                                --posPlayer;
                            else if (posBox > 0)
                            {
                                --posBox;
                                posPlayer = solutionPath[posBox].Length - 1;
                            }
                            else
                                ++posMove;
                            break;
                        case ConsoleKey.RightArrow:
                            ++posMove;
                            if (posPlayer < solutionPath[posBox].Length - 1)
                                ++posPlayer;
                            else if (posBox < solutionBox.Length - 1)
                            {
                                ++posBox;
                                posPlayer = 0;
                            }
                            else
                                --posMove;
                            break;
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Victory not found in {countTries} tries");
                Console.ReadKey();
            }

        }

        ushort[] BuildPath(ushort[] stateFrom, ushort[] stateTo)
        {
            ushort boxFromXY = stateTo[map.boxesCount];
            ushort boxToXY = boxFromXY;
            for (int i = 0; i < map.boxesCount; ++i)
                if (Array.BinarySearch(stateFrom, 0, map.boxesCount, stateTo[i]) < 0)
                {
                    boxToXY = stateTo[i];
                    break;
                }

            int boxFromX, boxFromY;
            int boxToX, boxToY;
            map.XY2Pos(boxFromXY, out boxFromX, out boxFromY);
            map.XY2Pos(boxToXY, out boxToX, out boxToY);

            int playerToX = (boxFromX << 1) - boxToX;
            int playerToY = (boxFromY << 1) - boxToY;
            ushort playerToXY = (ushort)(playerToX | (playerToY << 8));
            ushort playerFromXY = stateFrom[map.boxesCount];

            if (playerFromXY == playerToXY)
                return new ushort[] { playerToXY };

            explorer.ApplyState(stateFrom);
            explorer.Explore();

            return explorer.GetPath(playerToXY);
        }

        void DrawCell(ushort playerXY, int x, int y)
        {
            byte cell = explorer.GetCell(x, y);
            if ((cell & SokobanSolverMap.O_STONE) != 0)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("S ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                ushort xy = map.Pos2XY(x, y);
                bool isTarget = (cell & SokobanSolverMap.O_TARGET) != 0;

                if (isTarget)
                    Console.BackgroundColor = ConsoleColor.DarkYellow;

                if (playerXY == xy)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("P ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else if ((cell & SokobanSolverMap.O_BOX) != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("B ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(". ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                if (isTarget)
                    Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        public void Render(ushort[] state, ushort playerXY)
        {
            explorer.ApplyState(state);

            Console.SetCursorPosition(0, 2);
            for (int y = 0; y < map.height; ++y)
            {
                for (int x = 0; x < map.width; ++x)
                    DrawCell(playerXY, x, y);
                Console.WriteLine("");
            }
        }

    }
}
