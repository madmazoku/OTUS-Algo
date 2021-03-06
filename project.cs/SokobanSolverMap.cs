﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace project.cs
{
    class SokobanSolverMap
    {
        public const int MAX_WIDTH_HEIGHT = 1 << 8;
        public const byte O_EMPTY = 0b0000;
        public const byte O_STONE = 0b0001;
        public const byte O_TARGET = 0b0010;
        public const byte O_BOX = 0b0100;
        public const byte O_PLAYER = 0b1000;

        public string path;

        public int size;
        public int width;
        public int height;
        public int boxesCount;

        public byte[] cells;
        public ushort[] targetXYs;
        public ushort[] boxXYs;

        public ushort playerXY;

        public string lrud;

        public string Name { get { return Path.GetFileNameWithoutExtension(path); } }

        public SokobanSolverMap(SokobanSolverMap map)
        {
            path = map.path;

            size = map.size;
            width = map.width;
            height = map.height;
            boxesCount = map.boxesCount;

            cells = new byte[size];
            targetXYs = new ushort[map.boxesCount];
            boxXYs = new ushort[map.boxesCount];

            Array.Copy(map.cells, cells, map.size);
            Array.Copy(map.targetXYs, targetXYs, map.boxesCount);
            Array.Copy(map.boxXYs, boxXYs, map.boxesCount);

            playerXY = map.playerXY;
        }

        public SokobanSolverMap(string path)
        {
            ReadMap(path);
        }

        public void XY2Pos(ushort xy, out int x, out int y, out int pos)
        {
            x = xy & 0xff;
            y = xy >> 8;
            pos = x + y * width;
        }

        public void XY2Pos(ushort xy, out int x, out int y)
        {
            x = xy & 0xff;
            y = xy >> 8;
        }

        public int XY2Pos(int x, int y)
        {
            return x + y * width;
        }

        public ushort Pos2XY(int x, int y)
        {
            return (ushort)(x | (y << 8));
        }

        public int XY2Pos(ushort xy)
        {
            return (xy & 0xff) + (xy >> 8) * width;
        }

        const string VALID_MAP_CHARS = " .$*@+#";
        bool IsValidMapLine(string line)
        {
            foreach (char c in line)
                if (VALID_MAP_CHARS.IndexOf(c) == -1)
                    return false;
            return line.Length > 0;
        }

        void ReadMap(string path)
        {
            this.path = path;

            string[] lines = File.ReadAllLines(path).Where(x => IsValidMapLine(x)).ToArray();
            width = lines.Select(x => x.Length).Max();
            height = lines.Length;

            if (width > MAX_WIDTH_HEIGHT || height > MAX_WIDTH_HEIGHT)
                throw new Exception("Too big map");
            size = width * height;

            cells = new byte[size];
            List<ushort> targetXYList = new List<ushort>();
            List<ushort> boxXYList = new List<ushort>();

            for (int y = 0; y < height; ++y)
                for (int x = 0; x < lines[y].Length; ++x)
                {
                    int pos = XY2Pos(x, y);
                    ushort xy = Pos2XY(x, y);
                    switch (lines[y][x])
                    {
                        case ' ': break;
                        case '.': targetXYList.Add(xy); cells[pos] = O_TARGET; break;
                        case '$': boxXYList.Add(xy); cells[pos] = O_BOX; break;
                        case '*': targetXYList.Add(xy); boxXYList.Add(xy); cells[pos] = O_TARGET | O_BOX; break;
                        case '@': playerXY = xy; cells[pos] = O_PLAYER; break;
                        case '+': targetXYList.Add(xy); cells[pos] = O_TARGET; playerXY = xy; break;
                        case '#': cells[pos] = O_STONE; break;
                        default: break;
                    }
                }

            boxesCount = boxXYList.Count;

            targetXYs = targetXYList.ToArray();
            boxXYs = boxXYList.ToArray();

            Array.Sort<ushort>(targetXYs);
            Array.Sort<ushort>(boxXYs);

            if (File.Exists(path + ".lrud"))
                lrud = File.ReadAllText(path + ".lrud");
            else
                lrud = null;
        }

        public void RenderMap(int ox = 0, int oy = 0, int sx = 0, int sy = 0)
        {
            RenderMap(boxXYs, playerXY, ox, oy, sx, sy);
        }

        public void RenderMap(ushort[] state, int ox = 0, int oy = 0, int sx = 0, int sy = 0)
        {
            RenderMap(state, state[boxesCount], ox, oy, sx, sy);
        }

        public void RenderMap(ushort[] state, ushort playerXY, int ox = 0, int oy = 0, int sx = 0, int sy = 0)
        {
            for (int y = 0; y < height; y++)
            {
                Console.SetCursorPosition(ox, oy + y);
                for (int x = 0; x < width; ++x)
                {
                    int pos = XY2Pos(x, y);
                    byte cell = cells[pos];
                    if ((cell & O_STONE) != 0)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("S ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        ushort xy = Pos2XY(x, y);
                        bool isTarget = (cell & O_TARGET) != 0;

                        if (isTarget)
                            Console.BackgroundColor = ConsoleColor.DarkYellow;

                        if (playerXY == xy)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("P ");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        else if ((state.Length == boxesCount || xy != state[boxesCount]) && state.Contains(xy))
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
                Console.Write("".PadRight(Math.Max(0, sx - width) << 1));
            }
            for (int y = height; y < sy; ++y)
            {
                Console.SetCursorPosition(ox, oy + y);
                Console.Write("".PadRight(Math.Max(sx, width) << 1));
            }
        }
    }
}
