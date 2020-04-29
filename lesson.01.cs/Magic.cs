using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace lesson._01.cs
{
    class Magic
    {
        public Magic()
        {
            Console.SetWindowSize(50, 25);
            Console.SetBufferSize(50, 25);
        }

        public void SpellCraft()
        {
            Cast("01", (x, y) => y < x);
            Cast("02", (x, y) => y == x);
            Cast("03", (x, y) => y == 24 - x);
            Cast("04", (x, y) => y < 30 - x);
            Cast("05", (x, y) => y == x / 2);
            Cast("06", (x, y) => y < 10 || x < 10);
            Cast("07", (x, y) => y > 15 && x > 15);
            Cast("08", (x, y) => y * x == 0);
            Cast("09", (x, y) => Math.Abs(y - x) > 10);
            Cast("10", (x, y) => x <= y && y >= x / 2);
            Cast("11", (x, y) => ((x + 21) % 22) * ((y + 21) % 22) == 0);
            Cast("12", (x, y) => x * x + y * y <= 400);
            Cast("13", (x, y) => Math.Abs(24 - x - y) < 5);
            Cast("14", (x, y) => (23 - x) * (23 - x) + (23 - y) * (23 - y) > 332);
            Cast("15", (x, y) => Math.Abs(Math.Abs(x - y) - 15) < 6);
            Cast("16", (x, y) => Math.Abs(x - y) < 10 && Math.Abs(x + y - 24) < 10);
            Cast("17", (x, y) => Math.Sin(x / 3.0) * 8 + 16 < y);
            Cast("18", (x, y) => (x < 2 || y < 2) && (x + y != 0));
            Cast("19", (x, y) => (x % 24) * (y % 24) == 0);
            Cast("20", (x, y) => (x + y) % 2 == 0);
            Cast("21", (x, y) => x % (y + 1) == 0);
            Cast("22", (x, y) => (24 - x + 2 * y) % 3 == 0);
            Cast("23", (x, y) => (3 * x + 2 * y) % 6 == 0);
            Cast("24", (x, y) => (24 - x - y) * (x - y) == 0);
            Cast("25", (x, y) => (x % 6) * (y % 6) == 0);
        }

        public void Cast(string name, Func<int, int, bool> spell)
        {
            Console.Title = name;
            Console.SetCursorPosition(0, 0);
            for(int y = 0; y < 25; y++)
            {
                for(int x = 0; x < 25; x++)
                {
                    Console.Write(spell(x, y) ? "# " : ". ");
                }
            }
            Console.ReadKey();
        }
    }
}
