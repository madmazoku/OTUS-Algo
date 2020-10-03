using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace project.cs
{
    class SokobanStepper
    {
        SokobanSolverMap map;

        BitArray boxes;
        BitArray accessible;

        int newStatesCount;
        ushort[][] newStates;
        ushort[] accessXYs;
        bool[] avaliable;

        int exploreCount;
        ushort[] explore;

        ushort[] state;

        public SokobanStepper(SokobanSolverMap map)
        {
            this.map = new SokobanSolverMap(map);

            boxes = new BitArray(map.width * map.height);
            accessible = new BitArray(map.width * map.height);

            newStatesCount = 0;
            newStates = new ushort[map.boxesCount * 4][];
            accessXYs = new ushort[map.boxesCount * 4];
            avaliable = new bool[map.boxesCount * 4];

            exploreCount = 0;
            explore = new ushort[map.width * map.height];

            state = null;
        }

        void FillBoxes()
        {
            boxes.SetAll(false);
            for (int i = 0; i < map.boxesCount; ++i)
                boxes.Set((state[i] & 0xff) + (state[i] >> 8) * map.width, true);
        }

        void CheckCell(int x, int y, out bool isStone, out bool isOccupied)
        {
            if (x < 0 || y < 0 || x >= map.width || y >= map.height)
            {
                isStone = true;
                isOccupied = true;
            }

            int pos = x + y * map.width;
            isStone = map.stones.Get(pos);
            isOccupied = isStone || boxes.Get(pos);
        }

        void AddNewState(int index, int x, int y, int dx, int dy)
        {
            int accessX = x - dx;
            int accessY = y - dy;

            int boxX = x + dx;
            int boxY = y + dy;

            ushort[] newState = new ushort[map.boxesCount + 1];

            Array.Copy(state, newState, map.boxesCount);
            newState[map.boxesCount] = newState[index];
            newState[index] = (ushort)(boxX | (boxY << 8));
            Array.Sort(newState, 0, map.boxesCount);

            newStates[newStatesCount] = newState;
            accessXYs[newStatesCount] = (ushort)(accessX | (accessY << 8));
            avaliable[newStatesCount] = false;

            ++newStatesCount;
        }

        bool FillStatesNewPerBox(int i)
        {
            ushort xy = state[i];
            int x = xy & 0xff;
            int y = xy >> 8;

            bool isLeftStone, isLeftOccupied;
            bool isRightStone,isRightOccupied;
            bool isUpStone, isUpOccupied;
            bool isDownStone, isDownOccupied;

            CheckCell(x - 1, y, out isLeftStone, out isLeftOccupied);
            CheckCell(x + 1, y, out isRightStone, out isRightOccupied);
            CheckCell(x , y-1, out isUpStone, out isUpOccupied);
            CheckCell(x , y+1, out isDownStone, out isDownOccupied);

            if(!(isLeftOccupied||isRightOccupied))
            {
                AddNewState(i, x, y, -1, 0);
                AddNewState(i, x, y, 1, 0);
            }

            if (!(isUpOccupied || isDownOccupied))
            {
                AddNewState(i, x, y, 0, -1);
                AddNewState(i, x, y, 0, 1);
            }

            if (!(isLeftOccupied || isRightOccupied) || !(isUpOccupied || isDownOccupied))
                return true;

            int pos = x + y * map.width;
            bool isTarget = map.targets.Get(pos);
            if (isTarget)
                return true;

            // immovable box: stones on 2 aligned sides
            if (isLeftStone && isUpStone || isUpStone && isRightStone || isRightStone && isDownStone || isDownStone && isLeftStone)
                return false;

            // immovable box: part of 2x2 occupied square
            int ox = x + (isLeftOccupied ? -1 : 1);
            int oy = y + (isUpOccupied ? -1 : 1);

            bool isDiagStone, isDiagOccupied;
            CheckCell(ox, oy, out isDiagStone, out isDiagOccupied);
            if (isDiagOccupied)
                return false;

            return true;
        }

        bool FillStatesNew()
        {
            newStatesCount = 0;
            for (int i = 0; i < map.boxesCount; ++i)
                if(!FillStatesNewPerBox(i))
                {
                    newStatesCount = 0;
                    return false;
                }
            return true;
        }

        void Explore(int x, int y)
        {
            if (x < 0 || x >= map.width || y < 0 || y >= map.height)
                return;
            int pos = x + y * map.width;
            if (map.stones.Get(pos) || boxes.Get(pos) || accessible.Get(pos))
                return;
            explore[exploreCount++] = (ushort)(x | (y << 8));
            accessible.Set(pos, true);
        }

        void FillAccess()
        {
            accessible.SetAll(false);
            exploreCount = 0;

            {
                ushort xy = state[map.boxesCount];
                int x = xy & 0xff;
                int y = xy >> 8;
                int pos = x + y * map.width;
                explore[exploreCount++] = state[map.boxesCount];
                accessible.Set(pos, true);
            }

            while (exploreCount > 0)
            {
                ushort xy = explore[--exploreCount];
                int x = xy & 0xff;
                int y = xy >> 8;

                Explore(x - 1, y);
                Explore(x + 1, y);
                Explore(x, y - 1);
                Explore(x, y + 1);
            }
        }

        void FillAvaliable()
        {
            for (int i = 0; i < newStatesCount; ++i)
            {
                ushort xy = accessXYs[i];
                int x = xy & 0xff;
                int y = xy >> 8;
                int pos = x + y * map.width;
                avaliable[i] = accessible.Get(pos);
            }
        }

        public void Next(ushort[] state)
        {
            this.state = state;
            FillBoxes();

            Render();

            if (!FillStatesNew())
                return;

            FillAccess();
            FillAvaliable();
        }

        public void Queue(Dictionary<ushort[], ushort[]> moves, Queue<ushort[]> states)
        {
            for (int i = 0; i < newStatesCount; ++i)
                if (avaliable[i])
                {
                    if (!moves.ContainsKey(newStates[i]))
                    {
                        moves.Add(newStates[i], state);
                        states.Enqueue(newStates[i]);
                    }
                    else
                    {
                        Console.Write("");
                    }
                }
        }

        void DrawCell(int x, int y)
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

                if (boxes.Get(pos))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("B ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else if (state[map.boxesCount] == xy)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("P ");
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

        static int cnt = 0;
        public void Render()
        {
            if (++cnt < 100000)
                return;
            cnt = 0;
            Console.SetCursorPosition(0, 1);
            for (int y = 0; y < map.height; ++y)
            {
                for (int x = 0; x < map.width; ++x)
                    DrawCell(x, y);
                Console.WriteLine("");
            }
        }

        public void RenderAccess()
        {
            cnt = 0;
            Console.SetCursorPosition(0, 1);
            Console.ForegroundColor = ConsoleColor.White;
            for (int y = 0; y < map.height; ++y)
                for (int x = 0; x < map.width; ++x)
                    if(accessible.Get(x + y * map.width))
                    {
                        Console.SetCursorPosition((x << 1) + 1, y + 1);
                        Console.Write('+');
                    }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

    }
}
