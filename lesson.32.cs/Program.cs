using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;

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

                BloomFilter bf = new BloomFilter(wordPresent.Length << 1, errorRate);
                Console.WriteLine($"For capacity of {wordPresent.Length + wordAbsent.Length} keys with error rate {errorRate} there were {bf.ByteSize()} bytes allocated");

                foreach (string key in wordPresent)
                    bf.Add(key);

                int truePositiveCount = 0;
                int falsePositiveCount = 0;

                Console.WriteLine("");
                Console.WriteLine($"Present keys check ({wordPresent.Length}):");
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

                Console.WriteLine("");
                Console.WriteLine($"Absent keys check ({wordAbsent.Length}):");
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
                Console.WriteLine("");
                Console.WriteLine($"False positive rate: { 100.0 * falsePositiveRate:g3}%");

                Debug.Assert(truePositiveCount == wordPresent.Length, "no false negative for stored keys");
                Debug.Assert(falsePositiveRate <= errorRate, "false positive rate inside requested range");

            }
        }

        static List<string> GetWordsIteratorFromZipURI(string uri)
        {
            WebClient wc = new WebClient();
            byte[] data = wc.DownloadData(uri);
            MemoryStream ms = new MemoryStream(data);
            ZipArchive za = new ZipArchive(ms);
            Stream stream = za.Entries[0].Open();
            StreamReader reader = new StreamReader(stream);
            Regex regex = new Regex(@"\b(\w+)\b", RegexOptions.Compiled);
            List<string> words = new List<string>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine().ToLower();
                MatchCollection matches = regex.Matches(line);
                foreach (Match match in matches)
                    words.Add(match.Value);
            }
            return words;
        }

        static void HyperLogLogTest()
        {
            {
                UInt64[] hashes = new UInt64[]
                {
                    0b01 + (0b101111 << 2),
                    0b11 + (0b100010 << 2),
                    0b10 + (0b111000 << 2),
                    0b01 + (0b100011 << 2),
                    0b01 + (0b010110 << 2),
                    0b01 + (0b100000 << 2),
                    0b01 + (0b110010 << 2),
                    0b00 + (0b100100 << 2),
                };
                HyperLogLog hll = new HyperLogLog(2, 6);
                foreach (UInt64 hash in hashes)
                    hll.AddHash(hash);
                int count = hll.Count;
                Debug.Assert(count == 18, "check uniq count estimation");
            }
            {
                Console.WriteLine("");

                string[] sources = new string[] {
                    "https://fanfics.me/download.php?fic=68682&format=txt",
                    "https://fanfics.me/download.php?fic=84524&format=txt",
                    "https://fanfics.me/download.php?fic=82401&format=txt",
                    "https://fanfics.me/download.php?fic=87277&format=txt",
                    "https://fanfics.me/download.php?fic=111618&format=txt",
                    "https://fanfics.me/download.php?fic=50275&format=txt",
                    "https://fanfics.me/download.php?fic=132592&format=txt",
                    "full"
                };

                List<string>[] keyss = new List<string>[sources.Length];
                for (int source = 0; source < sources.Length; ++source)
                    keyss[source] = new List<string>();

                Console.WriteLine($"Read test sources");
                for (int source = 0; source < sources.Length - 1; ++source)
                {
                    Console.WriteLine($"Source: {sources[source]}");

                    Stopwatch sw;

                    sw = Stopwatch.StartNew();
                    Console.Write($"\tRead...           ");
                    keyss[source] = GetWordsIteratorFromZipURI(sources[source]);
                    sw.Stop();
                    Console.WriteLine($"spent: {sw.Elapsed.TotalSeconds}");

                    sw = Stopwatch.StartNew();
                    Console.Write($"\tAppend to full... ");
                    keyss[keyss.Length - 1].AddRange(keyss[source]);
                    sw.Stop();
                    Console.WriteLine($"spent: {sw.Elapsed.TotalSeconds}");
                }

                Console.WriteLine("");
                for (int source = 0; source < sources.Length; ++source)
                {
                    Console.WriteLine($"Source: {sources[source]}");

                    Stopwatch sw;

                    sw = Stopwatch.StartNew();
                    Console.Write($"\tCount overalls...          ");
                    int count = 0;
                    foreach (string key in keyss[source])
                        ++count;
                    sw.Stop();
                    Console.WriteLine($"spent: {sw.Elapsed.TotalSeconds}");

                    sw = Stopwatch.StartNew();
                    Console.Write($"\tCount real uniqs...        ");
                    HashSet<string> uniqs = new HashSet<string>();
                    foreach (string key in keyss[source])
                        uniqs.Add(key);
                    sw.Stop();
                    Console.WriteLine($"spent: {sw.Elapsed.TotalSeconds}");

                    sw = Stopwatch.StartNew();
                    Console.Write($"\tCount approximate uniqs... ");
                    HyperLogLog hll = new HyperLogLog(8, 12);
                    foreach (string key in keyss[source])
                        hll.Add(key);
                    sw.Stop();
                    Console.WriteLine($"spent: {sw.Elapsed.TotalSeconds}");

                    double errorPercent = 100.0 * Math.Abs(uniqs.Count - hll.Count) / uniqs.Count;

                    Console.WriteLine("");
                    Console.WriteLine($"\tOverall count         : {count}");
                    Console.WriteLine($"\tReal uniq count       : {uniqs.Count}");
                    Console.WriteLine($"\tApproximate uniq count: {hll.Count}; error: { errorPercent:g3}%");
                    Console.WriteLine("");

                    Debug.Assert(errorPercent < 30, "check error ratio");
                }
            }
        }

        static void Main(string[] args)
        {
            //Console.WriteLine("HyperLogLog/CountMin Sketch");
            BloomFilterTests();
            HyperLogLogTest();
        }
    }
}
