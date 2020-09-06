using System;
using System.Linq;

namespace lesson._27.cs
{
    class Peas
    {
        static int GCD(int a, int b)
        {
            if (a == b || b == 0)
                return a;
            if (a == 0)
                return b;

            if ((a & 0x1) == 0)
            {
                if ((b & 0x1) == 0)
                    return GCD(a >> 1, b >> 1) << 1;
                else
                    return GCD(a >> 1, b);
            }
            else
            {
                if ((b & 0x1) == 0)
                    return GCD(a, b >> 1);
                else
                {
                    if (a > b)
                        return GCD((a - b) >> 1, b);
                    else
                        return GCD(a, (b - a) >> 1);
                }
            }
        }

        static public void Do()
        {
            int[] args = Console.ReadLine().Trim().Split(new char[] { '+', '/' }).Select(x => int.Parse(x)).ToArray();
            int n = args[0] * args[3] + args[2] * args[1];
            int d = args[1] * args[3];
            int g = GCD(n, d);
            n /= g;
            d /= g;
            Console.WriteLine($"{n}/{d}");
        }
    }
}
