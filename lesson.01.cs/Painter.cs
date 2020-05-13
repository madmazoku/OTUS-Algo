using System;

namespace lesson._01.cs
{
    class Painter
    {
        int[,] map;
        int w, h;
        Random rand;
        string symbols = " #<>^vx";

        public Painter(int _w, int _h)
        {
            w = _w;
            h = _h;
            map = new int[w, h];
            rand = new Random();

            Console.SetWindowSize(w, h);
            Console.SetBufferSize(w, h);
        }

        public void RandomFill()
        {
            int x = rand.Next(w);
            int y = rand.Next(h);
            SetMap(x, y, 0);
            Fill(x, y);
        }

        public void Fill(int x, int y)
        {
            if (!IsEmpty(x, y)) return;

            SetMap(x, y, 2); Fill(x - 1, y);
            SetMap(x, y, 4); Fill(x, y - 1);
            SetMap(x, y, 3); Fill(x + 1, y);
            SetMap(x, y, 5); Fill(x, y + 1);
            SetMap(x, y, 6);
        }

        public void PutRandomNumbers()
        {
            SetMap(rand.Next(w), rand.Next(h), 1);
        }

        public void SetMap(int x, int y, int number)
        {
            map[x, y] = number;
            PrintAt(x, y);
        }

        void PrintAt(int x, int y)
        {
            int state = map[x, y];
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = state < 2 ? ConsoleColor.Blue : state < 6 ? ConsoleColor.Red : ConsoleColor.Green;
            Console.Write(symbols[map[x, y]]);
        }

        bool IsEmpty(int x, int y)
        {
            if (x < 0 || x >= w) return false;
            if (y < 0 || y >= h) return false;
            return map[x, y] == 0;
        }
    }
}
