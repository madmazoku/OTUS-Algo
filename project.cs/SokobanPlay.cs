using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        string solveLRUD;
        int solvePos;
        bool animate;

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

            solveLRUD = null;
            solvePos = 0;
            animate = false;

            if (File.Exists(path + ".lrud"))
                solveLRUD = File.ReadAllText(path + ".lrud");
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
            Console.SetCursorPosition(0, height + 1);
            Console.WriteLine("Use arrow keys to move player");
            Console.WriteLine("Use 'R' key to restart level");
            Console.WriteLine("Enter key to Restart level + animate solution if exists");
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


        public void Run()
        {
            Console.Clear();

            Render();

            while (true)
            {
                ConsoleKeyInfo cki;

                if (animate)
                {
                    Thread.Sleep(100);
                    if (solvePos < solveLRUD.Length)
                    {
                        switch (solveLRUD[solvePos++])
                        {
                            case 'L':
                                cki = new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false);
                                break;
                            case 'R':
                                cki = new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false);
                                break;
                            case 'U':
                                cki = new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false);
                                break;
                            case 'D':
                                cki = new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false);
                                break;
                            default:
                                throw new Exception("Unknown key in LRUD solve sequence");
                        }
                    }
                    else
                        cki = new ConsoleKeyInfo('\0', ConsoleKey.Spacebar, false, false, false);
                }
                else
                    cki = Console.ReadKey(true);

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
                            Render();
                            break;
                        case ConsoleKey.Enter:
                            if (!animate)
                            {
                                ReadMap();
                                Render();
                                if (solveLRUD != null)
                                {
                                    animate = true;
                                    Task.Run(() => { Console.ReadKey(true); animate = false; });
                                }
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    animate = false;
                    Console.Beep();
                    Console.SetCursorPosition(0, height + 5);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {e.Message}");
                }

                if (!animate && IsEqual(box, target))
                {
                    DrawVictory();
                    Console.ReadKey(true);
                    break;
                }
            }

            Console.Clear();
        }
    }
}
