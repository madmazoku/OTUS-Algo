using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace project.cs
{
    class SokobanEdit
    {
        string path;
        byte width;
        byte height;

        bool[,] stones;

        bool hasPlayer;
        ushort playerXY;
        List<ushort> targetBoxXYs;
        List<ushort> currentBoxXYs;

        byte cursorX;
        byte cursorY;
        string errorMsg;
        string statusMsg;

        public SokobanEdit(byte width, byte height, string path)
        {
            this.path = path;
            this.width = width;
            this.height = height;

            stones = new bool[width, height];

            hasPlayer = false;
            playerXY = 0;
            targetBoxXYs = new List<ushort>();
            currentBoxXYs = new List<ushort>();

            cursorX = 0;
            cursorY = 0;
            errorMsg = "";
            statusMsg = "";

            if (File.Exists(path))
                ReadMap();
        }

        void ReadMap()
        {
            string[] lines = File.ReadAllLines(path).Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
            byte mapWidth = (byte)lines[0].Length;
            byte mapHeight = (byte)lines.Length;

            if (width < mapWidth || height < mapHeight)
                throw new Exception("Too big map for load");

            byte offsetX = (byte)((width - mapWidth) >> 1);
            byte offsetY = (byte)((height - mapHeight) >> 1);

            hasPlayer = false;
            targetBoxXYs.Clear();
            currentBoxXYs.Clear();

            for (byte y = 0; y < mapHeight; ++y)
                for (byte x = 0; x < mapWidth; ++x)
                {
                    byte ox = (byte)(x + offsetX);
                    byte oy = (byte)(y + offsetY);
                    switch (lines[y][x])
                    {
                        case 'S':
                            stones[ox, oy] = true;
                            break;
                        case '.':
                            stones[ox, oy] = false;
                            break;
                        default:
                            ushort oxy = (ushort)((oy << 8) | ox);
                            switch (lines[y][x])
                            {
                                case 'T':
                                    targetBoxXYs.Add(oxy);
                                    break;
                                case 'B':
                                    currentBoxXYs.Add(oxy);
                                    break;
                                case 'P':
                                    hasPlayer = true;
                                    playerXY = oxy;
                                    break;
                            }
                            break;
                    }
                }
        }

        void WriteMap()
        {
            if (!hasPlayer)
                throw new Exception("Player position not set");
            if (targetBoxXYs.Count != currentBoxXYs.Count)
                throw new Exception($"Number of targets ({targetBoxXYs.Count}) not match number of boxes ({currentBoxXYs.Count})");

            byte minX = byte.MaxValue;
            byte minY = byte.MaxValue;
            byte maxX = 0;
            byte maxY = 0;

            for (byte y = 0; y < height; ++y)
                for (byte x = 0; x < width; ++x)
                {
                    bool touched;
                    if (stones[x, y])
                        touched = true;
                    else
                    {
                        ushort xy = (ushort)((y << 8) | x);
                        touched = (hasPlayer && playerXY == xy) || currentBoxXYs.Contains(xy) || targetBoxXYs.Contains(xy);
                    }
                    if (touched)
                    {
                        if (x < minX) minX = x;
                        if (x > maxX) maxX = x;
                        if (y < minY) minY = y;
                        if (y > maxY) maxY = y;
                    }
                }

            string tmpPath = Path.ChangeExtension(path, "tmp");
            StreamWriter sw = File.CreateText(tmpPath);
            for (byte y = minY; y <= maxY; ++y)
            {
                for (byte x = minX; x <= maxX; ++x)
                {
                    if (stones[x, y])
                        sw.Write('S');
                    else
                    {
                        ushort xy = (ushort)((y << 8) | x);
                        if (hasPlayer && playerXY == xy)
                            sw.Write('P');
                        else if (currentBoxXYs.Contains(xy))
                            sw.Write('B');
                        else if (targetBoxXYs.Contains(xy))
                            sw.Write('T');
                        else
                            sw.Write('.');
                    }
                }
                sw.Write("\n");
            }

            sw.Flush();
            sw.Close();

            File.Move(tmpPath, path, true);
        }

        byte SumWrap(int a, byte max, int d)
        {
            int o = d + a;
            while (o < 0)
                o += max;
            while (o >= max)
                o -= max;
            return (byte)o;
        }

        void Move(int dx, int dy)
        {
            bool[,] stonesNew = new bool[width, height];
            ushort playerXYNew = 0;
            List<ushort> targetBoxXYsNew = new List<ushort>();
            List<ushort> currentBoxXYsNew = new List<ushort>();

            for (byte y = 0; y < height; ++y)
                for (byte x = 0; x < width; ++x)
                {
                    byte nx = SumWrap(x, width, dx);
                    byte ny = SumWrap(y, height, dy);
                    stonesNew[nx, ny] = stones[x, y];
                }

            foreach (ushort xy in targetBoxXYs)
            {
                byte nx = SumWrap(xy & 0xff, width, dx);
                byte ny = SumWrap(xy >> 8, height, dy);
                targetBoxXYsNew.Add((ushort)((ny << 8) | nx));
            }

            foreach (ushort xy in currentBoxXYs)
            {
                byte nx = SumWrap(xy & 0xff, width, dx);
                byte ny = SumWrap(xy >> 8, height, dy);
                currentBoxXYsNew.Add((ushort)((ny << 8) | nx));
            }

            if (hasPlayer)
            {
                byte nx = SumWrap(playerXY & 0xff, width, dx);
                byte ny = SumWrap(playerXY >> 8, height, dy);
                playerXYNew = (ushort)((ny << 8) | nx);
            }

            stones = stonesNew;
            playerXY = playerXYNew;
            targetBoxXYs = targetBoxXYsNew;
            currentBoxXYs = currentBoxXYsNew;
        }

        void SetStone(byte x, byte y)
        {
            ushort xy = (ushort)((y << 8) | x);
            if (hasPlayer && playerXY == xy)
                throw new Exception("Can't set stone, player here");
            if (targetBoxXYs.Contains(xy))
                throw new Exception("Can't set stone, target here");
            if (currentBoxXYs.Contains(xy))
                throw new Exception("Can't set stone, box here");
            stones[x, y] = true;
        }

        void SetTarget(byte x, byte y)
        {
            ushort xy = (ushort)((y << 8) | x);
            if (targetBoxXYs.Contains(xy))
                return;
            if (hasPlayer && playerXY == xy)
                throw new Exception("Can't set target, player here");
            if (stones[x, y])
                throw new Exception("Can't set target, stone here");
            if (currentBoxXYs.Contains(xy))
                throw new Exception("Can't set target, box here");
            targetBoxXYs.Add(xy);
        }

        void SetBox(byte x, byte y)
        {
            ushort xy = (ushort)((y << 8) | x);
            if (currentBoxXYs.Contains(xy))
                return;
            if (stones[x, y])
                throw new Exception("Can't set box, stone here");
            if (hasPlayer && playerXY == xy)
                throw new Exception("Can't set box, player here");
            if (targetBoxXYs.Contains(xy))
                throw new Exception("Can't set box, target here");
            currentBoxXYs.Add(xy);
        }

        void SetPlayer(byte x, byte y)
        {
            ushort xy = (ushort)((y << 8) | x);
            if (hasPlayer && playerXY == xy)
                return;
            if (stones[x, y])
                throw new Exception("Can't set box, stone here");
            if (targetBoxXYs.Contains(xy))
                throw new Exception("Can't set stone, target here");
            if (currentBoxXYs.Contains(xy))
                throw new Exception("Can't set stone, box here");
            playerXY = xy;
            hasPlayer = true;
        }

        void SetEmpty(byte x, byte y)
        {
            ushort xy = (ushort)((y << 8) | x);
            if (hasPlayer && playerXY == xy)
                hasPlayer = false;
            stones[x, y] = false;
            targetBoxXYs.Remove(xy);
            currentBoxXYs.Remove(xy);
        }

        void FillPlace(ConsoleColor fc, char c)
        {
            Console.ForegroundColor = fc;
            Console.Write(c);
            Console.Write(' ');
        }

        static string[] legend = {
            "Legend:",
            "\tMovement control",
            "\t\tArrow keys: move cursor",
            "\t\tCapsLock + Arrow keys: shift map",
            "\tFill cell",
            "\t\tSpacebar: erase",
            "\t\tS: stone",
            "\t\tB: box",
            "\t\tT: target for box",
            "\t\tP: player",
            "\tStorage",
            "\t\tR: read",
            "\t\tW: write"
        };

        void Render()
        {
            Console.CursorVisible = false;

            for (int l = 0; l < legend.Length; ++l)
            {
                Console.SetCursorPosition((width << 1) + 2, l);
                Console.Write(legend[l]);
            }

            Console.SetCursorPosition(0, 0);
            for (byte y = 0; y < height; ++y)
            {
                for (byte x = 0; x < width; ++x)
                {
                    if (stones[x, y])
                        FillPlace(ConsoleColor.Gray, 'S');
                    else
                    {
                        ushort xy = (ushort)((y << 8) | x);
                        if (hasPlayer && playerXY == xy)
                            FillPlace(ConsoleColor.Red, 'P');
                        else if (currentBoxXYs.Contains(xy))
                            FillPlace(ConsoleColor.Yellow, 'B');
                        else if (targetBoxXYs.Contains(xy))
                            FillPlace(ConsoleColor.DarkYellow, 'T');
                        else
                            FillPlace(ConsoleColor.DarkGray, '.');
                    }
                }
                Console.Write("\n");
            }
            Console.Write('\n');

            Console.SetCursorPosition(0, height + 1);
            if (errorMsg.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(errorMsg.Substring(0, Math.Min(errorMsg.Length, Console.BufferWidth)).PadRight(Console.BufferWidth));
            }
            else
                Console.Write(errorMsg.PadRight(Console.BufferWidth));

            Console.SetCursorPosition(0, height + 2);
            if (statusMsg.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(statusMsg.Substring(0, Math.Min(statusMsg.Length, Console.BufferWidth)).PadRight(Console.BufferWidth));
            }
            else
                Console.Write(statusMsg.PadRight(Console.BufferWidth));

            Console.SetCursorPosition(cursorX << 1, cursorY);

            Console.ResetColor();
            Console.CursorVisible = true;
        }

        public void Run()
        {
            Console.Title = "Sokoban: Start Edit mode";

            int newWidth = (width << 1) + 50;
            int newHeight = height + 4;

            if (newWidth > Console.BufferWidth)
                Console.WindowWidth = Console.BufferWidth = newWidth;
            else
                Console.BufferWidth = Console.WindowWidth = newWidth;

            if (newHeight > Console.BufferHeight)
                Console.WindowHeight = Console.BufferHeight = newHeight;
            else
                Console.BufferHeight = Console.WindowHeight = newHeight;

            while (true)
            {
                Render();

                ConsoleKeyInfo cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.Escape)
                    break;

                errorMsg = "";

                try
                {
                    switch (cki.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            if (Console.CapsLock)
                                Move(-1, 0);
                            else if (cursorX > 0)
                                --cursorX;
                            statusMsg = "Moved left";
                            break;
                        case ConsoleKey.RightArrow:
                            if (Console.CapsLock)
                                Move(1, 0);
                            else if (cursorX < width - 1)
                                ++cursorX;
                            statusMsg = "Moved right";
                            break;
                        case ConsoleKey.UpArrow:
                            if (Console.CapsLock)
                                Move(0, -1);
                            else if (cursorY > 0)
                                --cursorY;
                            statusMsg = "Moved up";
                            break;
                        case ConsoleKey.DownArrow:
                            if (Console.CapsLock)
                                Move(0, 1);
                            else if (cursorY < height - 1)
                                ++cursorY;
                            statusMsg = "Moved down";
                            break;
                        case ConsoleKey.S:
                            SetStone(cursorX, cursorY);
                            statusMsg = "Stone set";
                            break;
                        case ConsoleKey.T:
                            SetTarget(cursorX, cursorY);
                            statusMsg = "Target set";
                            break;
                        case ConsoleKey.B:
                            SetBox(cursorX, cursorY);
                            statusMsg = "Box set";
                            break;
                        case ConsoleKey.P:
                            SetPlayer(cursorX, cursorY);
                            statusMsg = "Player set";
                            break;
                        case ConsoleKey.Spacebar:
                            SetEmpty(cursorX, cursorY);
                            statusMsg = "Cell erased";
                            break;
                        case ConsoleKey.R:
                            ReadMap();
                            statusMsg = "Reload map";
                            break;
                        case ConsoleKey.W:
                            WriteMap();
                            statusMsg = "Write map";
                            break;
                    }
                }
                catch (Exception e)
                {
                    errorMsg = e.Message;
                    Console.Beep();
                }
            }

            Console.Clear();

            Console.Title = "Sokoban: Leave Edit mode";

        }

    }
}
