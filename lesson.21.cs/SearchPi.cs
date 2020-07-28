using System;

namespace lesson._21.cs
{
    class SearchPi
    {
        string _pattern;
        int[] _pi;

        public SearchPi(string pattern)
        {
            _pattern = pattern;
            _pi = ComputePi();

            Console.WriteLine("\nPi");
            for (int i = 0; i < _pattern.Length; ++i)
                Console.WriteLine($"{i,3}: {_pi[i],3}");
            Console.WriteLine("\n");
        }

        public int Do(string text)
        {
            int n = text.Length;
            int q = 0;

            for (int i = 1; i < n; ++i)
            {
                while (q > 0 && text[i] != _pattern[q])
                    q = _pi[q - 1];
                if (text[i] == _pattern[q])
                    ++q;
                if (q == _pattern.Length)
                    return i - q + 1;
            }

            return -1;
        }


        private int[] ComputePi() { return ComputePiFast(); }

        private int[] ComputePiSlow()
        {
            int m = _pattern.Length;
            int[] pi = new int[m];

            for (int q = 0; q < m; ++q)
            {
                string line = _pattern.Left(q + 1);
                int rank = 0;
                for (int i = 1; i < q + 1; ++i)
                    if (line.Left(i) == line.Right(i))
                        rank = i;
                pi[q] = rank;
            }

            return pi;
        }

        private int[] ComputePiFast()
        {
            int n = _pattern.Length;
            int[] pi = new int[n];

            pi[0] = 0;

            for (int i = 1; i < n; ++i)
            {
                int q = pi[i - 1];
                while (q > 0 && _pattern[i] != _pattern[q])
                    q = pi[q - 1];
                if (_pattern[i] == _pattern[q])
                    ++q;
                pi[i] = q;
            }

            return pi;
        }
    }
}
