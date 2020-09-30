using System;
using System.IO;

namespace project.cs
{
    class Program
    {

        static string GetLessonsDataPath(int lesson)
        {
            string lessonDir = $"//lesson.{lesson:d2}.data";
            DirectoryInfo path = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (path != null)
            {
                if (Directory.Exists(path.ToString() + lessonDir))
                {
                    return path.ToString() + lessonDir;
                }
                path = path.Parent;
            }
            throw new Exception($"{lessonDir} directory not found through the parents of current directory");
        }

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

        static void Main(string[] args)
        {
            string levelDirectory = FindLevelDirectory();

            SokobanEdit se = new SokobanEdit(32, 32, Path.Combine(levelDirectory, "0001.txt"));
            se.Run();
        }
    }
}
