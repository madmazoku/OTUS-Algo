using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace project.cs
{
    class SokobanStepper
    {
        SokobanSolverMap map;
        SokobanSolverExplorer explorer;

        ushort[] state;

        int newStatesCount;
        ushort[][] newStates;
        ushort[] accessXYs;

        int[] distance;
        int[] order;

        public SokobanStepper(SokobanSolverMap map)
        {
            this.map = new SokobanSolverMap(map);
            explorer = new SokobanSolverExplorer(this.map);

            newStatesCount = 0;
            newStates = new ushort[map.boxesCount * 4][];
            accessXYs = new ushort[map.boxesCount * 4];

            distance = new int[map.boxesCount * 4];
            order = new int[map.boxesCount * 4];

            state = null;
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
            newState[index] = map.Pos2XY(boxX, boxY);
            Array.Sort(newState, 0, map.boxesCount);

            newStates[newStatesCount] = newState;
            accessXYs[newStatesCount] = map.Pos2XY(accessX,accessY);
            distance[newStatesCount] = -1;

            ++newStatesCount;
        }

        bool FillStatesNewPerBox(int i)
        {
            ushort xy = state[i];
            int x, y;
            map.XY2Pos(xy, out x, out y);

            bool isLeftStone, isLeftOccupied;
            bool isRightStone,isRightOccupied;
            bool isUpStone, isUpOccupied;
            bool isDownStone, isDownOccupied;

            explorer.CheckCell(x - 1, y, out isLeftStone, out isLeftOccupied);
            explorer.CheckCell(x + 1, y, out isRightStone, out isRightOccupied);
            explorer.CheckCell(x , y-1, out isUpStone, out isUpOccupied);
            explorer.CheckCell(x , y+1, out isDownStone, out isDownOccupied);

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
            if (explorer.IsOccupied(ox, oy))
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

        void FillAvaliable()
        {
            explorer.Explore();

            int x, y, pos;
            for (int i = 0; i < newStatesCount; ++i)
            {
                map.XY2Pos(accessXYs[i], out x, out y, out pos);
                distance[i] = explorer.distance[pos];
                order[i] = i;
            }
            Array.Sort(distance, order, 0, newStatesCount);
        }

        public void Next(ushort[] state)
        {
            this.state = state;
            explorer.SetState(state);

            if (!FillStatesNew())
                return;

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
