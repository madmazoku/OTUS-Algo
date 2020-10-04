using System;

namespace project.cs
{
    class SokobanSolverExplorer
    {
        public const byte O_EXPLORED = 0b10000;
        const byte O_UNEXPLORABLE_MASK = SokobanSolverMap.O_STONE | SokobanSolverMap.O_BOX | O_EXPLORED;

        SokobanSolverMap map;

        ushort[] state;

        public byte[] cells;
        public int[] distance;
        public ushort[] backtrace;

        FixedQueue<ushort> explore;
        FixedStack<ushort> path;

        public SokobanSolverExplorer(SokobanSolverMap map)
        {
            this.map = map;

            state = null;

            cells = new byte[map.size];
            Array.Copy(map.cells, cells, map.size);

            distance = new int[map.size];
            backtrace = new ushort[map.size];

            explore = new FixedQueue<ushort>(map.size);
            path = new FixedStack<ushort>(map.size);
        }

        void EraseState()
        {
            if (state == null)
                return;

            for (int i = 0; i < map.boxesCount; ++i)
                cells[map.XY2Pos(state[i])] &= ~SokobanSolverMap.O_BOX & 0xff;
            cells[map.XY2Pos(state[map.boxesCount])] &= ~SokobanSolverMap.O_PLAYER & 0xff;
        }

        void EraseExplored()
        {
            for (int i = 0; i < map.size; ++i)
                cells[i] &= ~O_EXPLORED & 0xff;
        }

        void SetState()
        {
            for (int i = 0; i < map.boxesCount; ++i)
                cells[map.XY2Pos(state[i])] |= SokobanSolverMap.O_BOX;
            cells[map.XY2Pos(state[map.boxesCount])] |= SokobanSolverMap.O_PLAYER;
        }

        public void ApplyState(ushort[] state)
        {
            EraseState();
            this.state = state;
            SetState();
        }

        void ExploreNext(int x, int y, ushort prevXY, int newDist)
        {
            if (x < 0 || x >= map.width || y < 0 || y >= map.height)
                return;

            int pos = map.XY2Pos(x, y);
            if ((cells[pos] & O_UNEXPLORABLE_MASK) != 0)
                return;

            cells[pos] |= O_EXPLORED;
            distance[pos] = newDist;
            backtrace[pos] = prevXY;

            explore.Enqueue(map.Pos2XY(x, y));
        }

        void ExploreStart()
        {
            EraseExplored();

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
            while (explore.Count > 0)
            {
                ushort xy = explore.Dequeue();
                int x, y, pos;
                map.XY2Pos(xy, out x, out y, out pos);
                int dist = distance[pos] + 1;

                ExploreNext(x - 1, y, xy, dist);
                ExploreNext(x + 1, y, xy, dist);
                ExploreNext(x, y - 1, xy, dist);
                ExploreNext(x, y + 1, xy, dist);
            }
        }

        public byte GetCell(int x, int y)
        {
            if (x < 0 || y < 0 || x >= map.width || y >= map.height)
                return SokobanSolverMap.O_STONE;
            return cells[map.XY2Pos(x, y)];
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

            while (true)
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
