using System;
using System.IO;

namespace project.cs
{
    class Program
    {

        static string FindLevelDirectory(string path = "")
        {
            if (path.Length == 0)
                path = Directory.GetCurrentDirectory();

            DirectoryInfo dir = new DirectoryInfo(path);
            while (dir != null)
            {
                string levelsPath = Path.Combine(dir.ToString(), "Levels");
                if (Directory.Exists(levelsPath))
                    return levelsPath;
                dir = dir.Parent;
            }
            throw new Exception($"\"Levels\" directory not found from {path} and up");
        }

        static void UpdateWindowSize(int width, int height)
        {
            if (width > Console.BufferWidth)
                Console.WindowWidth = Console.BufferWidth = width;
            else
                Console.BufferWidth = Console.WindowWidth = width;

            if (height > Console.BufferHeight)
                Console.WindowHeight = Console.BufferHeight = height;
            else
                Console.BufferHeight = Console.WindowHeight = height;
        }

        static void Main(string[] args)
        {
            UpdateWindowSize(160, 40);
            Console.CursorVisible = false;

            string levelDirectory = FindLevelDirectory();

            SokobanMenu sm = new SokobanMenu(levelDirectory);
            sm.Run();

            Console.CursorVisible = true;
        }
    }
}
