using System;
using System.Collections.Generic;

namespace project.cs
{
    class SokobanStepper
    {
        const byte O_OCCUPIED_MASK = SokobanSolverMap.O_STONE | SokobanSolverMap.O_BOX;

        SokobanSolverMap map;
        SokobanSolverExplorer explorer;

        ushort[] state;

        int newStatesCount;
        ushort[][] newStates;
        ushort[] accessXYs;

        int[] distance;
        int[] order;
        int newStatesAvaliableCount;

        public SokobanStepper(SokobanSolverMap map)
        {
            this.map = new SokobanSolverMap(map);
            explorer = new SokobanSolverExplorer(this.map);

            newStatesCount = 0;
            newStates = new ushort[map.boxesCount * 4][];
            accessXYs = new ushort[map.boxesCount * 4];

            newStatesAvaliableCount = 0;
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
            accessXYs[newStatesCount] = map.Pos2XY(accessX, accessY);

            ++newStatesCount;
        }

        bool FillStatesNewPerBox(int i)
        {
            ushort xy = state[i];
            int x, y, pos;
            map.XY2Pos(xy, out x, out y, out pos);

            byte leftCell = explorer.GetCell(x - 1, y);
            byte rightCell = explorer.GetCell(x + 1, y);
            byte upCell = explorer.GetCell(x, y - 1);
            byte downCell = explorer.GetCell(x, y + 1);

            bool IsHorizontalFree = ((leftCell | rightCell) & O_OCCUPIED_MASK) == 0;
            bool IsVerticalFree = ((upCell | downCell) & O_OCCUPIED_MASK) == 0;

            if (IsHorizontalFree)
            {
                AddNewState(i, x, y, -1, 0);
                AddNewState(i, x, y, 1, 0);
            }

            if (IsVerticalFree)
            {
                AddNewState(i, x, y, 0, -1);
                AddNewState(i, x, y, 0, 1);
            }

            if (IsHorizontalFree || IsVerticalFree)
                return true;

            if ((explorer.cells[pos] & SokobanSolverMap.O_TARGET) != 0)
                return true;

            // immovable box: stones on 2 aligned sides
            if ((((leftCell & upCell) | (upCell & rightCell) | (rightCell & downCell) | (downCell & leftCell)) & SokobanSolverMap.O_STONE) != 0)
                return false;

            // immovable box: part of 2x2 occupied square
            int ox = x + ((leftCell & O_OCCUPIED_MASK) != 0 ? -1 : 1);
            int oy = y + ((upCell & O_OCCUPIED_MASK) != 0 ? -1 : 1);
            if ((explorer.GetCell(ox, oy) & O_OCCUPIED_MASK) != 0)
                return false;

            return true;
        }

        bool FillStatesNew()
        {
            newStatesCount = 0;
            newStatesAvaliableCount = 0;
            for (int i = 0; i < map.boxesCount; ++i)
                if (!FillStatesNewPerBox(i))
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
                if ((explorer.cells[pos] & SokobanSolverExplorer.O_EXPLORED) == 0)
                    continue;

                distance[newStatesAvaliableCount] = explorer.distance[pos];
                order[newStatesAvaliableCount] = i;
                ++newStatesAvaliableCount;
            }
            Array.Sort(distance, order, 0, newStatesAvaliableCount);
        }

        public void Next(ushort[] state)
        {
            this.state = state;
            explorer.ApplyState(state);

            if (!FillStatesNew())
                return;

            FillAvaliable();
        }

        public void Queue(Dictionary<ushort[], ushort[]> moves, Queue<ushort[]> states)
        {
            for (int i = 0; i < newStatesAvaliableCount; ++i)
            {
                ushort[] newState = newStates[order[i]];
                if (!moves.ContainsKey(newState))
                {
                    moves.Add(newState, state);
                    states.Enqueue(newState);
                }
            }
        }

    }
}
