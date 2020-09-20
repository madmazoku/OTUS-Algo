using System;
using System.Diagnostics;

namespace lesson._32.cs
{
    class Program
    {
        static void BloomFilterTests()
        {
            Console.WriteLine("Bloom filter check");
            {
                BloomFilter bf = new BloomFilter(UInt16.MaxValue, 0.1);
                bf.Add("abc");
                Debug.Assert(bf.Exists("abc"), "simple existance check");
                Debug.Assert(!bf.Exists("cba"), "simple absence check");
            }
            {
                BloomFilter bf = new BloomFilter(UInt16.MaxValue, 0.1);

                for (int i = 0; i < UInt16.MaxValue; ++i)
                    bf.Add($"+{i}+");

                int truePositive = 0;
                int falseNegative = 0;
                int trueNegative = 0;
                int falsePositive = 0;

                for (int i = 0; i < UInt16.MaxValue; ++i)
                {
                    if (bf.Exists($"+{i}+"))
                        ++truePositive;
                    else
                        ++falseNegative;

                    if (bf.Exists($"-{i}-"))
                        ++falsePositive;
                    else
                        ++trueNegative;
                }

                Debug.Assert(truePositive == UInt16.MaxValue, "true existance check");
                Debug.Assert(falseNegative == 0, " false absence check");

                Debug.Assert(trueNegative >= UInt16.MaxValue * 0.9, "true absence check");
                Debug.Assert(falsePositive <= UInt16.MaxValue * 0.1, "false existance check");
            }
            {
                string[] wordPresent = new string[] { 
                    "abound","abounds","abundance","abundant","accessable",
                    "bloom","blossom","bolster","bonny","bonus","bonuses",
                    "coherent","cohesive","colorful","comely","comfort",
                    "gems","generosity","generous","generously","genial" 
                };
                string[] wordAbsent = new string[] {
                    "bluff","cheater","hate","war","humanity",
                    "racism","hurt","nuke","gloomy","facebook",
                    "geeksforgeeks","twitter"
                };
                double errorRate = 0.1;

                BloomFilter bf = new BloomFilter(wordPresent.Length, errorRate);
                Console.WriteLine($"For capacity of {wordPresent.Length} keys with error rate {errorRate} there were {bf.ByteSize()} bytes allocated");

                foreach (string key in wordPresent)
                    bf.Add(key);

                int truePositiveCount = 0;
                int falsePositiveCount = 0;

                Console.WriteLine("Present keys check:");
                ConsoleColor foreground = Console.ForegroundColor;
                foreach (string key in wordPresent)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"\t{key,15}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (bf.Exists(key))
                    {
                        ++truePositiveCount;
                        Console.WriteLine($": true positive");
                    }
                    else
                        Console.WriteLine($": false negative");
                }

                Console.WriteLine("Absent keys check:");
                foreach (string key in wordAbsent)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"\t{key,15}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (bf.Exists(key))
                    {
                        ++falsePositiveCount;
                        Console.WriteLine($": true positive");
                    }
                    else
                        Console.WriteLine($": false negative");
                }

                double falsePositiveRate = (double)falsePositiveCount / (truePositiveCount + falsePositiveCount);
                Console.WriteLine($"False positive rate: { 100.0 * falsePositiveRate,7:g3}%");

                Debug.Assert(truePositiveCount == wordPresent.Length, "no false negative for stored keys");
                Debug.Assert(falsePositiveRate <= errorRate, "false positive rate inside requested range");

            }
        }

        static void Main(string[] args)
        {
            //Console.WriteLine("HyperLogLog/CountMin Sketch");
            BloomFilterTests();
        }
    }
}
