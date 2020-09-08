using System;
using System.Collections.Generic;
using System.Text;

namespace lesson._29.cs
{
    class RomanToInteger
    {
        public int Do(string s)
        {
            string[] roman = new string[] { "CM", "CD", "XC", "XL", "IX", "IV", "M", "D", "C", "L", "X", "V", "I" };
            int[] arab = new int[] { 900, 400, 90, 40, 9, 4, 1000, 500, 100, 50, 10, 5, 1 };
            int N = 0;
            int pos;
            for (int j = 0; j < roman.Length; ++j)
                while (0 <= (pos = s.IndexOf(roman[j])))
                {
                    s = s.Remove(pos, roman[j].Length);
                    N += arab[j];
                }
            return N;
        }

    }
}
