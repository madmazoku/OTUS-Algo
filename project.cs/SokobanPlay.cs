using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace project.cs
{
    class SokobanPlay
    {
        const int MAX_WIDTH_HEIGHT = 1 << 8;

        string path;

        int width;
        int height;
        BitArray stone;
        BitArray box;
        BitArray target;
        int playerX;
        int playerY;

        public SokobanPlay(string path)
        {
            if (!File.Exists(path))
                throw new Exception("File not found");

            this.path = path;

            ReadMap();
        }

        const string VALID_MAP_CHARS = " .$*@+#";
        bool IsValidMapLine(string line)
        {
            foreach (char c in line)
                if (VALID_MAP_CHARS.IndexOf(c) == -1)
                    return false;
            return line.Length > 0;
        }

        void ReadMap()
        {
            string[] lines = File.ReadAllLines(path).Where(x => IsValidMapLine(x)).ToArray();
            width = (byte)lines.Select(x => x.Length).Max();
            height = (byte)lines.Length;

            if (width > MAX_WIDTH_HEIGHT || height > MAX_WIDTH_HEIGHT)
                throw new Exception("Too big map");

            int size = width * height;
            stone = new BitArray(size);
            box = new BitArray(size);
            target = new BitArray(size);

            for (int y = 0; y < height; ++y)
                for (int x = 0; x < lines[y].Length; ++x)
                {
                    int pos = x + y * width;
                    switch (lines[y][x])
                    {
                        case ' ': break;
                        case '.': target.Set(pos, true); break;
                        case '$': box.Set(pos, true); break;
                        case '*': box.Set(pos, true); target.Set(pos, true); break;
                        case '@': playerX = x; playerY = y; break;
                        case '+': playerX = x; playerY = y; target.Set(pos, true); break;
                        case '#': stone.Set(pos, true); break;
                        default: break;
                    }
                }
        }

        void DrawCell(int x, int y, bool locate)
        {
            if (locate)
                Console.SetCursorPosition(x << 1, y);

            int pos = x + y * width;
            if (stone.Get(pos))
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
                bool isTarget = target.Get(pos);

                if (isTarget)
                    Console.BackgroundColor = ConsoleColor.DarkYellow;

                if (box.Get(pos))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("B ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else if (playerX == x && playerY == y)
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

        void Render()
        {
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                    DrawCell(x, y, false);
                Console.WriteLine("");
            }
        }

        bool IsEqual(BitArray a, BitArray b)
        {
            for (int i = 0; i < width * height; ++i)
                if (a.Get(i) ^ b.Get(i))
                    return false;
            return true;
        }

        void DrawVictory()
        {
            Console.Clear();
            string msg = "Congratulation!";
            int offsetX = (Console.BufferWidth - 2 - msg.Length) >> 1;
            int offsetY = (Console.BufferHeight - 2) >> 1;
            Console.SetCursorPosition(offsetX, offsetY);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(msg);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        void Move(int dx, int dy)
        {
            int playerXOld = playerX;
            int playerYOld = playerY;
            int playerXNew = playerX + dx;
            int playerYNew = playerY + dy;
            if (playerXNew < 0 || playerXNew >= width || playerYNew < 0 || playerYNew >= height)
                throw new Exception("Can't go out of map");

            int posNew = playerXNew + playerYNew * width;
            if (stone.Get(posNew))
                throw new Exception("Can't go to stone");

            if (box.Get(posNew))
            {
                int boxXNew = playerXNew + dx;
                int boxYNew = playerYNew + dy;
                if (boxXNew < 0 || boxXNew >= width || boxYNew < 0 || boxYNew >= height)
                    throw new Exception("Can't push box out of map");

                int boxPosNew = boxXNew + boxYNew * width;
                if (stone.Get(boxPosNew))
                    throw new Exception("Can't push box to stone");
                if (box.Get(boxPosNew))
                    throw new Exception("Can't push box to box");

                box.Set(posNew, false);
                box.Set(boxPosNew, true);

                DrawCell(boxXNew, boxYNew, true);
            }

            playerX = playerXNew;
            playerY = playerYNew;

            DrawCell(playerXOld, playerYOld, true);
            DrawCell(playerXNew, playerYNew, true);
        }

        void UpdateWindowSize()
        {
            int newWidth = (width << 1) + 1;
            int newHeight = height + 1;

            if (newWidth > Console.BufferWidth)
                Console.WindowWidth = Console.BufferWidth = newWidth;
            else
                Console.BufferWidth = Console.WindowWidth = newWidth;

            if (newHeight > Console.BufferHeight)
                Console.WindowHeight = Console.BufferHeight = newHeight;
            else
                Console.BufferHeight = Console.WindowHeight = newHeight;
        }

        public void Run()
        {
            Console.Title = "Sokoban: Enter Play mode";
            Console.CursorVisible = false;

            UpdateWindowSize();

            Render();

            while (true)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Escape)
                    break;

                try
                {
                    switch (cki.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            Move(-1, 0);
                            break;
                        case ConsoleKey.RightArrow:
                            Move(1, 0);
                            break;
                        case ConsoleKey.UpArrow:
                            Move(0, -1);
                            break;
                        case ConsoleKey.DownArrow:
                            Move(0, 1);
                            break;
                        case ConsoleKey.R:
                            ReadMap();
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.Beep();
                }

                if (IsEqual(box, target))
                {
                    DrawVictory();
                    Console.ReadKey(true);
                    break;
                }
            }

            Console.ResetColor();
            Console.CursorVisible = true;
            Console.Title = "Sokoban: Leave Play mode";
        }
    }
}
