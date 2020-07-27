using System;

namespace lesson._20.cs
{
    class Program
    {

        static int Find_Native(string text, string pattern)
        {
            for (int i = 0; i < text.Length - pattern.Length + 1; ++i) {
                bool found = true;
                for(int j = 0; j < pattern.Length; ++j)
                    if(text[i+j] != pattern[j])
                    {
                            found = false;
                            break;
                    }
                if (found)
                    return i;
            }
            return -1;
        }

        static int Find_0(string text, string pattern)
        {
            int i = 0;
            while(i < text.Length - pattern.Length + 1)
            {
                int j = 0;
                while(j < pattern.Length && text[i + j] == pattern[j])
                    ++j;
                if (j == pattern.Length)
                    return i;
                ++i;
            }
            return -1;
        }

        static int[] CreatePrefixes(string pattern)
        {
            int[] prefixes = new int[char.MaxValue + 1];
            Array.Fill(prefixes, -1);
            for (int i = 0; i < pattern.Length - 1; ++i)
                prefixes[pattern[i]] = i;
            return prefixes;
        }

        static int[] CreateSuffixes(string pattern)
        {
            int[] suffixes = new int[pattern.Length + 1];
            Array.Fill(suffixes, pattern.Length, 0, pattern.Length);
            suffixes[pattern.Length] = 1;

            for(int i = pattern.Length - 1; i >= 0; --i)
            {
                for(int j = i; j < pattern.Length; ++j)
                {
                    string suffix = pattern.Substring(j);
                    for(int k = j - 1; k >= 0; --k)
                    {
                        string prefix = pattern.Substring(k, suffix.Length);
                        if(suffix == prefix)
                        {
                            suffixes[i] = j-k;
                            j = pattern.Length;
                            break;
                        }
                    }
                }
            }

            return suffixes;
        }

        static int Find_1(string text, string pattern)
        {
            int[] prefixes = CreatePrefixes(pattern);

            int last = pattern.Length - 1;
            int i = 0;
            while (i < text.Length - last)
            {
                int j = last;
                while (j >= 0 && text[i + j] == pattern[j])
                    --j;
                if (j == -1)
                    return i;
                i += last - prefixes[text[i + last]];
            }

            return -1;
        }

        static int Find_2(string text, string pattern)
        {
            int[] prefixes = CreatePrefixes(pattern);
            int[] suffixes = CreateSuffixes(pattern);

            int last = pattern.Length - 1;
            int i = 0;
            while (i < text.Length - last)
            {
                int j = last;
                while (j >= 0 && text[i + j] == pattern[j])
                    --j;
                if (j == -1)
                    return i;
                i += Math.Max(j - prefixes[text[i + j]], suffixes[j + 1]);
            }

            return -1;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Алгоритм Бойера-Мура");

            string text = "SOMESTRING";
            string pattern = "STRING";
            //string pattern = "KOLOKOL";
            //string pattern = "abcdadcd";

            Console.WriteLine($"Find_Native(\"{text}\", \"{pattern}\") = {Find_Native(text, pattern)}");
            Console.WriteLine($"Find_0(\"{text}\", \"{pattern}\") = {Find_0(text, pattern)}");
            Console.WriteLine($"Find_1(\"{text}\", \"{pattern}\") = {Find_1(text, pattern)}");
            Console.WriteLine($"Find_2(\"{text}\", \"{pattern}\") = {Find_2(text, pattern)}");
        }
    }
}
