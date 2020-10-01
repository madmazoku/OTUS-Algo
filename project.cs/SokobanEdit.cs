using System;
using System.IO;
using System.Linq;

namespace project.cs
{
    class SokobanEdit
    {
        string path;
        int width;
        int height;

        const int MAX_WIDTH_HEIGHT = 1 << 8;
        const byte O_EMPTY = 0b0000;
        const byte O_STONE = 0b0001;
        const byte O_TARGET = 0b0010;
        const byte O_BOX = 0b0100;
        const byte O_PLAYER = 0b1000;

        byte[,] cells;

        byte cursorX;
        byte cursorY;
        string errorMsg;
        string statusMsg;

        public SokobanEdit(int width, int height, string path)
        {
            if (width < 0 || width > MAX_WIDTH_HEIGHT || height < 0 || height > MAX_WIDTH_HEIGHT)
                throw new Exception("Invalid size");

            this.path = path;
            this.width = width;
            this.height = height;

            cells = new byte[width, height];

            cursorX = 0;
            cursorY = 0;
            errorMsg = "";
            statusMsg = "";

            if (File.Exists(path))
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
            byte mapWidth = (byte)lines.Select(x => x.Length).Max();
            byte mapHeight = (byte)lines.Length;

            if (width < mapWidth || height < mapHeight)
                throw new Exception("Too big map for load");

            byte minX = (byte)((width - mapWidth) >> 1);
            byte minY = (byte)((height - mapHeight) >> 1);
            byte maxX = (byte)(minX + mapWidth);
            byte maxY = (byte)(minY + mapHeight);

            for (int y = 0; y < height; ++y)
                for (int x = 0; x < width; ++x)
                    if (y < minY || y >= maxY || x < minX || x >= lines[y - minY].Length + minX)
                        cells[x, y] = O_EMPTY;
                    else
                    {
                        switch (lines[y - minY][x - minX])
                        {
                            case ' ': cells[x, y] = O_EMPTY; break;
                            case '.': cells[x, y] = O_TARGET; break;
                            case '$': cells[x, y] = O_BOX; break;
                            case '*': cells[x, y] = O_BOX | O_TARGET; break;
                            case '@': cells[x, y] = O_PLAYER; break;
                            case '+': cells[x, y] = O_PLAYER | O_TARGET; break;
                            case '#': cells[x, y] = O_STONE; break;
                            default: break;
                        }
                    }
        }

        void WriteMap()
        {
            int playerCount = 0;
            int targetCount = 0;
            int boxCount = 0;

            int[] lineMaxXs = new int[height];
            int minX = byte.MaxValue;
            int minY = byte.MaxValue;
            int maxX = 0;
            int maxY = 0;


            for (int y = 0; y < height; ++y)
                for (int x = 0; x < width; ++x)
                {
                    byte cell = cells[x, y];
                    if (cell != O_EMPTY)
                    {
                        if (x > lineMaxXs[y]) lineMaxXs[y] = x;
                        if (x < minX) minX = x;
                        if (x > maxX) maxX = x;
                        if (y < minY) minY = y;
                        if (y > maxY) maxY = y;
                    }
                    if ((cell & O_PLAYER) == O_PLAYER) ++playerCount;
                    if ((cell & O_TARGET) == O_TARGET) ++targetCount;
                    if ((cell & O_BOX) == O_BOX) ++boxCount;
                }
            if (playerCount != 1)
                throw new Exception("Must be exact one player");
            if (targetCount == 0 || boxCount == 0)
                throw new Exception("Must be some target and boxes");
            if (targetCount != boxCount)
                throw new Exception("Must be the same number of target and boxes");

            string tmpPath = Path.ChangeExtension(path, "tmp");
            StreamWriter sw = File.CreateText(tmpPath);
            for (int y = minY; y <= maxY; ++y)
            {
                for (int x = minX; x <= lineMaxXs[y]; ++x)
                    switch (cells[x, y])
                    {
                        case O_EMPTY: sw.Write(' '); break;
                        case O_TARGET: sw.Write('.'); break;
                        case O_BOX: sw.Write('$'); break;
                        case O_BOX | O_TARGET: sw.Write('*'); break;
                        case O_PLAYER: sw.Write('@'); break;
                        case O_PLAYER | O_TARGET: sw.Write('+'); break;
                        case O_STONE: sw.Write('#'); break;
                        default: break;
                    }
                sw.Write("\n");
            }

            sw.Flush();
            sw.Close();

            File.Move(tmpPath, path, true);
        }

        byte SumWrap(int a, int max, int d)
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
            byte[,] cellsNew = new byte[width, height];

            for (int y = 0; y < height; ++y)
                for (int x = 0; x < width; ++x)
                {
                    byte nx = SumWrap(x, width, dx);
                    byte ny = SumWrap(y, height, dy);
                    cellsNew[nx, ny] = cells[x, y];
                }

            cells = cellsNew;
        }

        void Resize(int dx, int dy)
        {
            int widthNew = Math.Min(Math.Max(width + dx * 2, 0), MAX_WIDTH_HEIGHT);
            int heightNew = Math.Min(Math.Max(height + dy * 2, 0), MAX_WIDTH_HEIGHT);

            int sx = Math.Min(widthNew, width);
            int sy = Math.Min(heightNew, height);

            int dxN = Math.Max(dx, 0);
            int dyN = Math.Max(dy, 0);

            int dxO = Math.Max(-dx, 0);
            int dyO = Math.Max(-dy, 0);

            byte[,] cellsNew = new byte[widthNew, heightNew];

            for (int y = 0; y < sy; ++y)
                for (int x = 0; x < sx; ++x)
                    cellsNew[x + dxN, y + dxN] = cells[x + dxO, y + dyO];

            cells = cellsNew;
            width = widthNew;
            height = heightNew;
        }

        void SetStone(int x, int y)
        {
            if ((cells[x, y] & (O_BOX | O_PLAYER | O_TARGET)) == O_EMPTY)
                cells[x, y] |= O_STONE;
            else
                throw new Exception("Can't set stone here");
        }

        void SetTarget(int x, int y)
        {
            if ((cells[x, y] & O_STONE) == O_EMPTY)
                cells[x, y] |= O_TARGET;
            else
                throw new Exception("Can't set target here");
        }

        void SetBox(int x, int y)
        {
            if ((cells[x, y] & (O_STONE | O_PLAYER)) == O_EMPTY)
                cells[x, y] |= O_BOX;
            else
                throw new Exception("Can't set box here");
        }

        void ErasePlayer()
        {
            for (int y = 0; y < width; ++y)
                for (int x = 0; x < height; ++x)
                    if ((cells[x, y] & O_PLAYER) != O_EMPTY)
                    {
                        cells[x, y] &= (byte)(~O_PLAYER & 0xff);
                        return;
                    }
        }

        void SetPlayer(int x, int y)
        {
            if ((cells[x, y] & (O_STONE | O_BOX)) == O_EMPTY)
            {
                ErasePlayer();
                cells[x, y] |= O_PLAYER;
            }
            else
                throw new Exception("Can't set player here");
        }

        void SetEmpty(int x, int y)
        {
            cells[x, y] = O_EMPTY;
        }

        void FillCell(ConsoleColor fc, char c)
        {
            Console.ForegroundColor = fc;
            Console.Write(c);
            Console.Write(' ');
        }

        static string[] legend = {
            "Legend:",
            "  Map control",
            "    Numpad Arrow keys: move cursor",
            "    CapsLock + Numpad Arrow keys: shift map",
            "    +: enlarge map",
            "    -: shrink map",
            "  Fill cell",
            "    Spacebar: erase",
            "    S: stone",
            "    B: box",
            "    T: target for box",
            "    P: player",
            "  Storage",
            "    R: read",
            "    W: write"
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
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    byte cell = cells[x, y];
                    if (cell == O_STONE)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        FillCell(ConsoleColor.Gray, 'S');
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        if ((cell & O_TARGET) == O_TARGET)
                            Console.BackgroundColor = ConsoleColor.DarkYellow;

                        if ((cell & O_BOX) == O_BOX)
                            FillCell(ConsoleColor.Yellow, 'B');
                        else if ((cell & O_PLAYER) == O_PLAYER)
                            FillCell(ConsoleColor.Red, 'P');
                        else
                            FillCell(ConsoleColor.Gray, '.');

                        if ((cell & O_TARGET) == O_TARGET)
                            Console.BackgroundColor = ConsoleColor.Black;

                    }
                }
                Console.Write("\n");
            }
            Console.Write('\n');

            if (errorMsg.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(errorMsg.Substring(0, Math.Min(errorMsg.Length, Console.BufferWidth)).PadRight(Console.BufferWidth));
            }
            else
                Console.WriteLine(errorMsg.PadRight(Console.BufferWidth));

            if (statusMsg.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(statusMsg.Substring(0, Math.Min(statusMsg.Length, Console.BufferWidth)).PadRight(Console.BufferWidth));
            }
            else
                Console.WriteLine(statusMsg.PadRight(Console.BufferWidth));

            Console.SetCursorPosition(cursorX << 1, cursorY);

            Console.ResetColor();
            Console.CursorVisible = true;
        }

        void UpdateWindowSize()
        {
            int legentLength = legend.Select(x => x.Length).Max();

            int newWidth = (width << 1) + legentLength + 4;
            int newHeight = height + 4;

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
            Console.Title = "Sokoban: Enter Edit mode";

            UpdateWindowSize();

            while (true)
            {
                Render();

                ConsoleKeyInfo cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Escape)
                    break;

                errorMsg = "";
                statusMsg = "";

                try
                {
                    switch (cki.Key)
                    {
                        case ConsoleKey.NumPad4:
                            //case ConsoleKey.LeftArrow:
                            if ((cki.Modifiers & ConsoleModifiers.Control) != 0)
                                Move(-1, 0);
                            else if (cursorX > 0)
                                --cursorX;
                            statusMsg = "Moved left";
                            break;
                        case ConsoleKey.NumPad6:
                            //case ConsoleKey.RightArrow:
                            if ((cki.Modifiers & ConsoleModifiers.Control) != 0)
                                Move(1, 0);
                            else if (cursorX < width - 1)
                                ++cursorX;
                            statusMsg = "Moved right";
                            break;
                        case ConsoleKey.NumPad8:
                            //case ConsoleKey.UpArrow:
                            if ((cki.Modifiers & ConsoleModifiers.Control) != 0)
                                Move(0, -1);
                            else if (cursorY > 0)
                                --cursorY;
                            statusMsg = "Moved up";
                            break;
                        case ConsoleKey.NumPad2:
                            //case ConsoleKey.DownArrow:
                            if ((cki.Modifiers & ConsoleModifiers.Control) != 0)
                                Move(0, 1);
                            else if (cursorY < height - 1)
                                ++cursorY;
                            statusMsg = "Moved down";
                            break;
                        case ConsoleKey.Add:
                            Resize(1, 1);
                            statusMsg = "Map enlarged";
                            UpdateWindowSize();
                            break;
                        case ConsoleKey.Subtract:
                            Resize(-1, -1);
                            statusMsg = "Map shrinked";
                            UpdateWindowSize();
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
