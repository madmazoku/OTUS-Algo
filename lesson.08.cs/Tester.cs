using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace lesson._08.cs
{
    class Tester
    {
        private string group;
        private DirectoryInfo tmpPath;
        private List<IFileSort> fileSorts;

        public int Count { get { return fileSorts.Count; } }

        public Tester(string group, long lesson)
        {
            this.group = group;

            tmpPath = Directory.CreateDirectory(Path.Combine("F://Temp", $"lessong.{lesson:d2}." + Path.GetRandomFileName()));
            //tmpPath = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), $"lessong.{lesson:d2}." + Path.GetRandomFileName()));
            fileSorts = new List<IFileSort>();

            Console.WriteLine($"Created temporary directory {tmpPath.FullName}");
        }

        ~Tester()
        {
            tmpPath.Delete(true);
            Console.WriteLine($"Removed temporary directory {tmpPath.FullName}");
        }

        public void Add(IFileSort fileSort)
        {
            fileSorts.Add(fileSort);
        }

        public void RunTests(long[] arraySizes)
        {
            Console.WriteLine($"{group}");

            long n = 0;
            while (File.Exists($"F://Temp/sorts.{n}.csv"))
                ++n;

            StreamWriter streamCSV = new StreamWriter($"F://Temp/sorts.{n}.csv");
            streamCSV.Write("arraySize");
            foreach (IFileSort fileSort in fileSorts)
                streamCSV.Write($",{fileSort.Name()}");
            streamCSV.Write("\n");


            for (int arraySizeIndex = 0; arraySizeIndex < arraySizes.Length; ++arraySizeIndex)
            {
                long arraySize = arraySizes[arraySizeIndex];

                int arrayExp = (int)Math.Floor(Math.Log10(arraySize));
                string pathSource = Path.Combine(tmpPath.ToString(), $"{arrayExp:d3}");

                Console.WriteLine($"\tArraySize: 10^{arrayExp} ({arraySize})");

                FileInfo fileSource = new FileInfo(pathSource + ".source.uint16");
                CreateSourceFile(fileSource, arraySize);

                CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3600));
                //CancellationTokenSource tokenSource = new CancellationTokenSource();

                List<(string, Task<(double, bool)>)> tasks = new List<(string, Task<(double, bool)>)>();
                foreach (IFileSort fileSort in fileSorts)
                {
                    FileInfo fileDestination = new FileInfo(pathSource + "." + fileSort.Name() + ".destination.uint16");

                    Task<(double, bool)> task = Task.Run(() =>
                    {
                        try
                        {
                            Stopwatch sw = Stopwatch.StartNew();
                            fileSort.Sort(fileSource, fileDestination, tokenSource.Token);
                            sw.Stop();
                            return (sw.Elapsed.TotalSeconds, false);
                        }
                        catch (OperationCanceledException)
                        {
                            return (0, true);
                        }
                    }, tokenSource.Token);

                    tasks.Add((fileSort.Name(), task));
                }

                streamCSV.Write($"{arraySize}");
                foreach ((string name, Task<(double, bool)> task) in tasks)
                {
                    Console.Write($"\t\t{name,30}: ");
                    task.Wait();
                    (double sec, bool timeout) = task.Result;
                    streamCSV.Write(","); if (!timeout) streamCSV.Write($"{sec}");
                    if (timeout)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write($"{"Timeout",10}");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                        Console.Write($"{sec,10:g8}");
                    Console.WriteLine("");
                }
                streamCSV.WriteLine("");
                streamCSV.Flush();
            }
            streamCSV.Close();
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
