using System;
using System.Data;
using System.IO;
using System.Linq;

namespace project.cs
{
    class SokobanMenu
    {
        const string selectMap = "Select Map:";
        const string newItem = "<New>";

        string levelsPath;
        SokobanSolverMap[] maps;
        int selectedMapPos;

        int maxMapNameLength;
        int maxWidth;
        int maxHeight;

        public SokobanMenu(string levelsPath)
        {
            this.levelsPath = levelsPath;
            maps = null;
            selectedMapPos = 0;

            maxMapNameLength = newItem.Length;
            maxWidth = 32;
            maxHeight = 32;
        }

        void LoadMaps()
        {
            maps = Directory.EnumerateFiles(levelsPath, "*.xsb").Select(x => new SokobanSolverMap(x)).OrderBy(x => x.Name).ToArray();
            selectedMapPos = 0;

            maxMapNameLength = Math.Max(selectMap.Length, maps.Select(x => x.Name.Length).Max() + 6);
            maxWidth = maps.Select(x => x.width).Max();
            maxHeight = maps.Select(x => x.width).Max();
        }

        void Render()
        {
            RenderMenu();
            RenderMap();
            RenderLegend();
        }

        void RenderMenu()
        {
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Select map:");
            int windowHeight = Math.Min(maps.Length + 1, Console.WindowHeight - 4);
            int menuOffset = Math.Min(0, maps.Length + 1 - selectedMapPos - windowHeight);
            for (int i = 0; i < windowHeight; ++i)
            {
                int mapPos = i + selectedMapPos + menuOffset;
                if (mapPos == selectedMapPos)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                string element;
                if (mapPos == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    element = newItem;
                }
                else
                {
                    SokobanSolverMap map = maps[mapPos - 1];
                    if (map.lrud == null)
                        element = map.Name;
                    else
                        element = $"{map.Name} [{map.lrud.Length,4}]";
                }

                Console.SetCursorPosition(0, i + 1);
                Console.Write(element.PadLeft(element.Length + ((maxMapNameLength - element.Length) >> 1)).PadRight(maxMapNameLength));
            }
            Console.ResetColor();
        }

        void RenderMap()
        {
            if (selectedMapPos == 0)
            {
                for (int y = 0; y < maxHeight; ++y)
                {
                    Console.SetCursorPosition(maxMapNameLength + 2, y);
                    Console.Write("".PadRight(maxWidth << 1));
                }
            }
            else
                maps[selectedMapPos - 1].RenderMap(maxMapNameLength + 2, 0, maxWidth, maxHeight);
        }

        void RenderLegend()
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 4);
            Console.WriteLine("Use Up/Down key to select desired level map");
            Console.WriteLine("Use Enter key to play; 'E' key to edit and 'S' key to solve level map");
        }

        void EditLevel(string mapName)
        {
            SokobanEdit edit = new SokobanEdit(Math.Max(maxWidth, maxHeight), Math.Max(maxWidth, maxHeight), Path.Combine(levelsPath, mapName + ".xsb"));
            edit.Run();
            LoadMaps();
        }

        public void Run()
        {
            Console.Clear();

            LoadMaps();

            while (true)
            {
                Render();
                ConsoleKeyInfo cki = Console.ReadKey(true);

                if (cki.Key == ConsoleKey.Escape)
                    break;

                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedMapPos > 0)
                            --selectedMapPos;
                        break;
                    case ConsoleKey.DownArrow:
                        if (selectedMapPos < maps.Length)
                            ++selectedMapPos;
                        break;
                    case ConsoleKey.Enter:
                        if (selectedMapPos > 0)
                        {
                            SokobanPlay play = new SokobanPlay(maps[selectedMapPos - 1].path);
                            play.Run();
                        }
                        else
                        {
                            Console.Clear();
                            Console.Write("Enter new level name >");
                            string mapName = Console.ReadLine();
                            if (mapName.Length > 0)
                                EditLevel(mapName);
                        }
                        break;
                    case ConsoleKey.E:
                        if (selectedMapPos > 0)
                        {
                            int pos = selectedMapPos;
                            EditLevel(maps[selectedMapPos - 1].Name);
                            selectedMapPos = pos;
                        }
                        else
                        {
                            Console.Clear();
                            Console.Write("Enter new level name >");
                            string mapName = Console.ReadLine();
                            if (mapName.Length > 0)
                                EditLevel(mapName);
                        }
                        break;
                    case ConsoleKey.S:
                        if (selectedMapPos > 0)
                        {
                            SokobanSolver solver = new SokobanSolver(maps[selectedMapPos - 1].path);
                            solver.Run();
                            LoadMaps();
                        }
                        break;
                }
            }

            Console.Clear();
        }

    }
}
