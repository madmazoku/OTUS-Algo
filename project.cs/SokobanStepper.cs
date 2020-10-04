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

        int newStatesCount;
        ushort[][] newStates;
        ushort[] accessXYs;

        FixedQueue<ushort> explore;
        int[] explored;
        int[] distance;
        int[] order;

        ushort[] state;

        public SokobanStepper(SokobanSolverMap map)
        {
            this.map = new SokobanSolverMap(map);

            boxes = new BitArray(map.width * map.height);

            newStatesCount = 0;
            newStates = new ushort[map.boxesCount * 4][];
            accessXYs = new ushort[map.boxesCount * 4];

            explore = new FixedQueue<ushort>(map.width * map.height);
            explored = new int[map.width * map.height];
            distance = new int[map.boxesCount * 4];
            order = new int[map.boxesCount * 4];

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
                return;
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
            distance[newStatesCount] = -1;

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

        void Explore(int x, int y, int dist)
        {
            if (x < 0 || x >= map.width || y < 0 || y >= map.height)
                return;
            int pos = x + y * map.width;
            if (map.stones.Get(pos) || boxes.Get(pos) || explored[pos] != -1)
                return;
            explore.Enqueue((ushort)(x | (y << 8)));
            explored[pos] = dist;
        }

        void FillAccess()
        {
            Array.Fill(explored, -1);
            explore.Clear();

            Explore(state[map.boxesCount] & 0xff, state[map.boxesCount] >> 8, 0);

            while (explore.Count > 0)
            {
                ushort xy = explore.Dequeue();
                int x = xy & 0xff;
                int y = xy >> 8;
                int pos = x + y * map.width;
                int dist = explored[pos] + 1;

                Explore(x-1, y, dist);
                Explore(x+1, y, dist);
                Explore(x, y-1, dist);
                Explore(x, y+1, dist);
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
                distance[i] = explored[pos];
                order[i] = i;
            }
            Array.Sort(distance, order, 0, newStatesCount);
        }

        public void Next(ushort[] state)
        {
            this.state = state;
            FillBoxes();

            if (!FillStatesNew())
                return;

            FillAccess();
            FillAvaliable();
        }

        public void Queue(Dictionary<ushort[], ushort[]> moves, Queue<ushort[]> states)
        {
            for (int i = 0; i < newStatesCount; ++i)
                if (distance[i] != -1)
                {
                    ushort[] newState = newStates[order[i]];
                    if (!moves.ContainsKey(newState))
                    {
                        moves.Add(newState, state);
                        states.Enqueue(newState);
                    }
                    else
                    {
                        Console.Write("");
                    }
                }
        }

    }
}
