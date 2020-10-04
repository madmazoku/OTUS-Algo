using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace project.cs
{
    class SokobanSolverExplorer
    {
        SokobanSolverMap map;

        ushort[] state;

        public BitArray boxes;
        public int[] distance;
        public ushort[] backtrace;

        FixedQueue<ushort> explore;
        FixedStack<ushort> path;

        public SokobanSolverExplorer(SokobanSolverMap map)
        {
            this.map = map;

            state = null;

            boxes = new BitArray(map.size);
            distance = new int[map.size];
            backtrace = new ushort[map.size];

            explore = new FixedQueue<ushort>(map.size);
            path = new FixedStack<ushort>(map.size);
        }

        public void SetState(ushort[] state)
        {
            this.state = state;

            boxes.SetAll(false);
            for (int i = 0; i < map.boxesCount; ++i)
                boxes.Set(map.XY2Pos(state[i]), true);
        }

        void ExploreNext(int x, int y, ushort prevXY, int newDist)
        {
            if (x < 0 || x >= map.width || y < 0 || y >= map.height)
                return;
            int pos = map.XY2Pos(x, y);
            if (map.stones.Get(pos) || boxes.Get(pos) || distance[pos] != -1)
                return;
            explore.Enqueue(map.Pos2XY(x,y));
            distance[pos] = newDist;
            backtrace[pos] = prevXY;
        }

        void ExploreStart()
        {
            Array.Fill(distance, -1);

            ushort xy = state[map.boxesCount];
            int pos = map.XY2Pos(xy);

            explore.Clear();
            explore.Enqueue(xy);
            distance[pos] = 0;
            backtrace[pos] = xy;
        }

        public void Explore()
        {
            ExploreStart();
            while(explore.Count > 0)
            {
                ushort xy = explore.Dequeue();
                int x, y, pos;
                map.XY2Pos(xy, out x, out y, out pos);
                int dist = distance[pos] + 1;

                ExploreNext(x - 1, y, xy, dist);
                ExploreNext(x +1, y, xy, dist);
                ExploreNext(x , y-1, xy, dist);
                ExploreNext(x , y+1, xy, dist);
            }
        }

        public void CheckCell(int x, int y, out bool isStone, out bool isOccupied)
        {
            if (x < 0 || y < 0 || x >= map.width || y >= map.height)
            {
                isStone = true;
                isOccupied = true;
                return;
            }

            int pos = map.XY2Pos(x,y);
            isStone = map.stones.Get(pos);
            isOccupied = isStone || boxes.Get(pos);
        }

        public bool IsOccupied(int x, int y)
        {
            if (x < 0 || y < 0 || x >= map.width || y >= map.height)
                return true;
            int pos = map.XY2Pos(x, y);
            return map.stones.Get(pos) || boxes.Get(pos);
        }

        public int GetDistance(ushort xy)
        {
            return distance[map.XY2Pos(xy)];
        }

        public ushort[] GetPath(ushort toXY)
        {
            ushort playerXY = state[map.boxesCount];
            ushort xy = toXY;
            path.Clear();

            while(true)
            {
                path.Push(xy);
                if (xy == playerXY)
                    break;
                xy = backtrace[map.XY2Pos(xy)];
            }

            return path.ToArray();
        }
    }
}
