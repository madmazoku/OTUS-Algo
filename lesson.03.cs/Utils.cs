using System;
using System.Linq;

namespace lesson._03.cs
{
    class Utils
    {
        public static int CountBits(ulong x)
        {
            int s = 0;
            while (x > 0)
            {
                x &= x - 1;
                ++s;
            }
            return s;
        }

        public static ulong[] ParseFEN(string input)
        {
            ulong[] positions = new ulong[12];
            ulong p = 1;
            foreach (string l in input.Split("/").Select(x => x.Trim()).Reverse())
            {
                foreach (char c in l)
                {
                    if (c >= '0' && c <= '9')
                        p <<= c - '0';
                    else
                    {
                        long piece = c < 'a' ? 0 : 6;
                        switch (Char.ToLower(c))
                        {
                            case 'p': piece += 0; break;
                            case 'n': piece += 1; break;
                            case 'b': piece += 2; break;
                            case 'r': piece += 3; break;
                            case 'q': piece += 4; break;
                            case 'k': piece += 5; break;
                            default: throw new Exception($"unknown piece type {c}");
                        }
                        positions[piece] |= p;
                        p <<= 1;
                    }
                }
            }

            if (p != 0) throw new Exception("invalid FEN string");

            return positions;
        }
    }
}
