using System;
using System.Collections.Generic;
using System.Text;

namespace lesson._29.cs
{
    class AddBinary
    {
        public string Do(string a, string b)
        {
            int ja = a.Length - 1;
            int jb = b.Length - 1;
            string answer = "";
            int p = 0;
            while (ja >= 0 || jb >= 0)
            {
                int s = digit(a, ja--) + digit(b, jb--) + p;
                answer = ((s & 0x1) == 1 ? '1' : '0') + answer;
                p = s >> 1;
            }
            if (p == 1)
                answer = '1' + answer;
            return answer;
        }

        int digit(string s, int j)
        {
            return j < 0 ? 0 : s[j] == '0' ? 0 : 1;
        }
    }
}
