using System;
using System.Collections.Generic;
using System.Linq;

namespace project.cs
{

    class SokobanSolver
    {
        SokobanSolverMap map;

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

                //Console.ReadKey(true);

                stepper.Queue(moves, states);
            }

            if(isVictory)
            {
                Console.Clear();
                Console.WriteLine($"Victory found in {countTries} tries");

                Stack<ushort[]> solutionList = new Stack<ushort[]>();
                ushort[] state = new ushort[map.boxesCount + 1];
                Array.Copy(victoryState, state, map.boxesCount + 1);

                while(!arrayComparer.Equals(startState, state))
                {
                    solutionList.Push(state);
                    if (!moves.TryGetValue(state, out state))
                        throw new Exception("broken solution");
                }
                solutionList.Push(startState);

                ushort[][] solution = solutionList.ToArray();

                int pos = 0;
                while (true)
                {
                    Render(solution[pos]);

                    ConsoleKeyInfo cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Escape)
                        break;

                    switch (cki.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            if (pos > 0)
                                --pos; ;
                            break;
                        case ConsoleKey.RightArrow:
                            if (pos < solution.Length - 1)
                                ++pos; ;
                            break;
                    }
                }
            } else
            {
                Console.Clear();
                Console.WriteLine($"Victory not found in {countTries} tries");
                Console.ReadKey();
            }

        }

        void DrawCell(ushort[] state, int x, int y)
        {
            int pos = x + y * map.width;
            if (map.stones.Get(pos))
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("S ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                ushort xy = (ushort)((y << 8) | x);
                bool isTarget = map.targets.Get(pos);

                if (isTarget)
                    Console.BackgroundColor = ConsoleColor.DarkYellow;

                if (state[map.boxesCount] == xy)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("P ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else if (state.Contains(xy))
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

        public void Render(ushort[] state)
        {
            Console.SetCursorPosition(0, 1);
            for (int y = 0; y < map.height; ++y)
            {
                for (int x = 0; x < map.width; ++x)
                    DrawCell(state, x, y);
                Console.WriteLine("");
            }
        }

    }
}
