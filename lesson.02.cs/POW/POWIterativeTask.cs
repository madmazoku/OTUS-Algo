namespace lesson._02.cs
{
    class POWIterativeTask : POWTask
    {

        public override string Name() { return "Итеративный"; }

        public override double POW(double x, long y)
        {
            double r = 1;
            while (y > 0)
            {
                r *= x;
                --y;
            }
            return r;
        }
    }
}
