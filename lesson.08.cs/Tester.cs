using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace lesson._08.cs
{
    class Tester
    {
        private string group;
        private DirectoryInfo tmpPath;
        private List<IFileSort> innerSorts;

        public int Count { get { return innerSorts.Count; } }

        public Tester(string group, long lesson)
        {
            this.group = group;

            //tmpPath = Directory.CreateDirectory(Path.Combine("E://Temp", $"lessong.{lesson:d2}." + Path.GetRandomFileName()));
            tmpPath = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), $"lessong.{lesson:d2}." + Path.GetRandomFileName()));
            innerSorts = new List<IFileSort>();

            Console.WriteLine($"Created temporary directory {tmpPath.FullName}");
        }

        ~Tester()
        {
            tmpPath.Delete(true);
            Console.WriteLine($"Removed temporary directory {tmpPath.FullName}");
        }

        public void Add(IFileSort innerSort)
        {
            innerSorts.Add(innerSort);
        }

        public void RunTests(long[] arraySizes)
        {
            Console.WriteLine($"{group}");

            for (int arraySizeIndex = 0; arraySizeIndex < arraySizes.Length; ++arraySizeIndex)
            {
                long arraySize = arraySizes[arraySizeIndex];

                Console.WriteLine($"\tArraySize: {arraySize}");

                int arrayExp = (int)Math.Floor(Math.Log10(arraySize));
                string pathSource = Path.Combine(tmpPath.ToString(), $"{arrayExp:d3}");

                FileInfo fileSource = new FileInfo(pathSource + ".source.uint16");
                CreateSourceFile(fileSource, arraySize);

                foreach (IFileSort innerSort in innerSorts)
                {
                    FileInfo fileDestination = new FileInfo(pathSource + "." + innerSort.Name() + ".destination.uint16");

                    Console.Write($"\t\t{innerSort.Name()}: ");

                    if (fileDestination.Exists)
                        fileDestination.Delete();

                    Stopwatch swSort = Stopwatch.StartNew();
                    innerSort.Sort(fileSource, fileDestination);
                    swSort.Stop();

                    Console.WriteLine($"Sort: {swSort.Elapsed.TotalSeconds,10:g8}; ");
                }

            }

            Console.WriteLine("");
        }


        void CreateSourceFile(FileInfo fileSource, long arraySize)
        {
            FileStream fileStream = fileSource.OpenWrite();

            Random rnd = new Random();
            byte[] buffer = new byte[sizeof(UInt16)];
            for (long index = 0; index < arraySize; ++index)
            {
                rnd.NextBytes(buffer);
                fileStream.Write(buffer);
            }

            fileStream.Close();
        }
    }
}
