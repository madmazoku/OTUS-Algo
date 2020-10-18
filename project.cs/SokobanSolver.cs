using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace project.cs
{

    class SokobanSolver
    {
        SokobanSolverMap map;
        SokobanSolverExplorer explorer;

        ArrayComparer<ushort> arrayComparer;

        Dictionary<ushort[], ushort[]> moves;
        Queue<ushort[]> states;

        ushort[][] solutionBox;
        ushort[][] solutionPath;
        int countMoves;

        public SokobanSolver(string path)
        {
            map = new SokobanSolverMap(path);

            arrayComparer = new ArrayComparer<ushort>();
            moves = new Dictionary<ushort[], ushort[]>(arrayComparer);
            states = new Queue<ushort[]>();

            explorer = new SokobanSolverExplorer(map);

            solutionBox = null;
            solutionPath = null;
            countMoves = 0;
        }

        ushort[] SolveStates()
        {
            map.RenderMap(0, 2);

            ushort[] victoryState = new ushort[map.boxesCount + 1];
            Array.Copy(map.targetXYs, victoryState, map.boxesCount);

            ushort[] startState = new ushort[map.boxesCount + 1];
            Array.Copy(map.boxXYs, startState, map.boxesCount);
            startState[map.boxesCount] = map.playerXY;

            states.Enqueue(startState);

            SokobanStepper stepper = new SokobanStepper(map);
            int countTries = 0;
            Stopwatch sw = Stopwatch.StartNew();
            double swPrevSec = 0;
            while (states.Count > 0)
            {
                ++countTries;

                double swCurSec = sw.Elapsed.TotalSeconds;
                if (swCurSec - swPrevSec > 1)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine($"spent sec: {Math.Round(swCurSec)}; states per sec: {Math.Ceiling(countTries / swCurSec)}; states to explore: {states.Count}".PadRight(Console.BufferWidth));
                    swPrevSec = swCurSec;
                }

                ushort[] state = states.Dequeue();
                victoryState[map.boxesCount] = state[map.boxesCount];
                if (arrayComparer.Equals(victoryState, state))
                    return victoryState;

                stepper.Next(state);
                stepper.Queue(moves, states);
            }
            return null;
        }

        void BuildSolution(ushort[] victoryState)
        {
            Stack<ushort[]> solutionBoxStack = new Stack<ushort[]>();
            ushort[] state = new ushort[map.boxesCount + 1];
            Array.Copy(victoryState, state, map.boxesCount + 1);

            ushort[] startState = new ushort[map.boxesCount + 1];
            Array.Copy(map.boxXYs, startState, map.boxesCount);
            startState[map.boxesCount] = map.playerXY;

            while (!arrayComparer.Equals(startState, state))
            {
                solutionBoxStack.Push(state);
                if (!moves.TryGetValue(state, out state))
                    throw new Exception("broken solution");
            }
            solutionBoxStack.Push(startState);

            solutionBox = solutionBoxStack.ToArray();
            solutionPath = new ushort[solutionBox.Length][];
            countMoves = 0;
            for (int i = 0; i < solutionBox.Length - 1; ++i)
            {
                ushort[] movePlayer = BuildPath(solutionBox[i], solutionBox[i + 1]);
                solutionPath[i] = movePlayer;
                countMoves += movePlayer.Length;
            }
            solutionPath[solutionBox.Length - 1] = new ushort[] { solutionBox[solutionBox.Length - 1][map.boxesCount] };
        }

        void WriteLRUD()
        {
            StringBuilder sb = new StringBuilder();
            int playerFromX, playerFromY;
            int playerToX, playerToY;
            map.XY2Pos(solutionPath[0][0], out playerFromX, out playerFromY);
            for (int i = 0; i < solutionPath.Length; ++i)
            {
                for (int j = 0; j < solutionPath[i].Length; ++j)
                {
                    if (i == 0 && j == 0)
                        continue;
                    map.XY2Pos(solutionPath[i][j], out playerToX, out playerToY);
                    if (playerToX - playerFromX == -1)
                        sb.Append('L');
                    else if (playerToX - playerFromX == 1)
                        sb.Append('R');
                    else if (playerToY - playerFromY == -1)
                        sb.Append('U');
                    else if (playerToY - playerFromY == 1)
                        sb.Append('D');
                    else
                        throw new Exception("invalid path");
                    playerFromX = playerToX;
                    playerFromY = playerToY;
                }
            }

            string lrudPath = map.path + ".lrud";
            File.WriteAllText(lrudPath, sb.ToString());
        }

        public void Run()
        {
            Console.Clear();

            solutionBox = null;
            solutionPath = null;

            Stopwatch sw = Stopwatch.StartNew();
            ushort[] victoryState = SolveStates();
            sw.Stop();

            if (victoryState == null)
            {
                Console.Clear();
                Console.WriteLine($"Victory not found in {sw.Elapsed.TotalSeconds} sec");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine($"Victory found in {sw.Elapsed.TotalSeconds} sec");

            BuildSolution(victoryState);
            WriteLRUD();

            Console.SetCursorPosition(0, 2 + map.height);
            Console.WriteLine("Use left and right arrow keys to explore solution");
            Console.WriteLine("Use Ctrl+left and Ctrl+right arrow keys to animate solution");

            int posMove = 0;
            int posBox = 0;
            int posPlayer = 0;
            int autoMove = 0;

            while (true)
            {
                Console.SetCursorPosition(0, 1);
                Console.Write($"At {posMove} move from {countMoves}".PadRight(Console.BufferWidth));
                map.RenderMap(solutionBox[posBox], solutionPath[posBox][posPlayer], 0, 2);
                Console.SetCursorPosition(0, 5 + map.height);

                ConsoleKeyInfo cki;
                switch (autoMove)
                {
                    case -1:
                        if (posMove == 1)
                            autoMove = 0;
                        cki = new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false);
                        break;
                    case 1:
                        if (posMove == countMoves - 1)
                            autoMove = 0;
                        cki = new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false);
                        break;
                    default:
                        cki = Console.ReadKey(true);
                        break;
                }

                if (cki.Key == ConsoleKey.Escape)
                    break;

                switch (cki.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if ((cki.Modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control)
                            autoMove = -1;
                        else
                        {
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
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if ((cki.Modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control)
                            autoMove = 1;
                        else
                        {
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
                        }
                        break;
                }
            }

            Console.Clear();
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

    }
}
