using System;
using System.IO;
using System.Threading;

namespace lesson._06.cs
{
    class Utils
    {
        public static string GetTestPath(long lesson, string testDir)
        {
            string lessonTestDir = $"//lesson.{lesson:d2}.data//{testDir}";
            DirectoryInfo path = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (path != null)
            {
                if (Directory.Exists(path.ToString() + lessonTestDir))
                {
                    return path.ToString() + lessonTestDir;
                }
                path = path.Parent;
            }
            throw new Exception($"{lessonTestDir} directory not found through the parents of current directory");
        }

        public static void Swap(int[] array, int leftIndex, int rightIndex, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            int t = array[leftIndex];
            array[leftIndex] = array[rightIndex];
            array[rightIndex] = t;
        }
    }
}
