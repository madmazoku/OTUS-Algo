using System;

namespace lesson._21.cs
{
    class SearchFSM
    {
        string _pattern;
        int[,] _delta;

        public SearchFSM(string pattern)
        {
            _pattern = pattern;
            _delta = ComputeDelta();

            Console.WriteLine("\ndelta");
            for (int row = 0; row < _pattern.Length; ++row)
                Console.WriteLine($"{row,3}: {_delta[row, 0],3} | {_delta[row, 1],3} |");
            Console.WriteLine("\n");
        }

        public int Do(string text)
        {
            int m = _pattern.Length;
            int n = text.Length;
            int q = 0;
            for (int i = 0; i < n; ++i)
            {
                q = _delta[q, text[i] - 'A'];
                if (q == m)
                    return i - m + 1;
            }

            return -1;
        }

        private int[,] ComputeDelta()
        {
            int m = _pattern.Length;
            int[,] delta = new int[m, 4];

            for (int q = 0; q < m; ++q)
                foreach (char c in "ABCD")
                {
                    string line = _pattern.Left(q) + c;
                    int k = q + 1;
                    while (_pattern.Left(k) != line.Right(k))
                        --k;
                    delta[q, c - 'A'] = k;
                }

            return delta;
        }
    }
}
