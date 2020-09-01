using System;
using System.Linq;
using System.Xml.Serialization;

namespace lesson._27.cs
{
    class Program
    {

        static public int GCD(int a, int b)
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

        static public void Peas()
        {
            int[] args = Console.ReadLine().Split(new char[]{ '+', '/' }).Select(x => int.Parse(x)).ToArray();
            int n = args[0] * args[3] + args[2] * args[1];
            int d = args[1] * args[3];
            int g = GCD(n, d);
            n /= g;
            d /= g;
            Console.WriteLine($"{n}/{d}");
        }

        static public void FiveEight()
        {
            long N = int.Parse(Console.ReadLine());
            long K;
            if (N <= 0)
                K = 0;
            else if (N == 1)
                K = 2;
            else if (N == 2)
                K = 4;
            else
            {
                long[] x = { 1, 1, 1, 1 };
                long[] n = { 0, 0, 0, 0 };
                while (N > 2)
                {
                    n[0] = x[1] + x[3];
                    n[1] = x[0] + x[2];
                    n[2] = x[0];
                    n[3] = x[1];
                    Array.Copy(n, x, 4);
                    --N;
                }
                K = x[0] + x[1] + x[2] + x[3];
            }
            Console.WriteLine(K);
        }

        static void Main(string[] args)
        {
            // Peas();
            FiveEight();
        }
    }
}
