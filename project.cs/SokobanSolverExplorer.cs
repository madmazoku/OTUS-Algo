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

        BitArray boxes;

        FixedQueue<ushort> explore;
        int[] distance;
        ushort[] backtrace;

        FixedStack<ushort> path;

        public SokobanSolverExplorer(SokobanSolverMap map)
        {
            this.map = map;

            int size = map.width * map.height;

            boxes = new BitArray(size);

            explore = new FixedQueue<ushort>(size);
            distance = new int[size];
            backtrace = new ushort[size];

            path = new FixedStack<ushort>(size);
        }

        public void SetState(ushort[] state)
        {
            this.state = state;

            boxes.SetAll(false);
            for (int i = 0; i < map.boxesCount; ++i)
            {
                ushort xy = state[i];
                int x = xy & 0xff;
                int y = xy >> 8;
                int pos = x + y * map.width;
                boxes.Set(pos, true);
            }
        }

        void ExploreNext(int x, int y, ushort prevXY, int newDist)
        {
            if (x < 0 || x >= map.width || y < 0 || y >= map.height)
                return;
            int pos = x + y * map.width;
            if (map.stones.Get(pos) || boxes.Get(pos) || distance[pos] != -1)
                return;
            explore.Enqueue((ushort)(x | (y << 8)));
            distance[pos] = newDist;
            backtrace[pos] = prevXY;
        }

        void ExploreStart()
        {
            Array.Fill(distance, -1);

            ushort xy = state[map.boxesCount];
            int x = xy & 0xff;
            int y = xy >> 8;
            int pos = x + y * map.width;

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

        public int GetDistance(ushort xy)
        {
            int x = xy & 0xff;
            int y = xy >> 8;
            int pos = x + y * map.width;
            return distance[pos];
        }

        public ushort[] GetPath(ushort toXY)
        {
            ushort playerXY = state[map.boxesCount];
            ushort xy = toXY;
            path.Clear();

            int x, y, pos;
            while(true)
            {
                path.Push(xy);
                if (xy == playerXY)
                    break;
                map.XY2Pos(xy, out x, out y, out pos);
                if (distance[pos] == -1)
                    throw new Exception("broken path");
                xy = backtrace[pos];
            }

            return path.ToArray();
        }
    }
}
