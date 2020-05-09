using System;
using System.Collections.Generic;
using System.Text;

namespace lesson._02.cs
{
    abstract class POWTask : ITask
    {
        private double x;
        private long y;
        private double pow;

        public abstract string Name();

        public void Prepare(string[] data)
        {
            x = double.Parse(data[0]);
            y = long.Parse(data[1]);
            pow = 0;
        }

        public void Run()
        {
            pow = POW(x, y);
        }

        public bool Result(string expected)
        {
            double expectedPOW = double.Parse(expected);
            return Math.Abs(pow - expectedPOW) <= 1e-6 * 0.5 * expectedPOW;
        }

        public abstract double POW(double x, long y);
    }
}
