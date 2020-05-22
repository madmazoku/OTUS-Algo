namespace lesson._06.cs
{
    class GonnetSequence : IShellSequence
    {
        public string Name() { return "Gonnet"; }

        public int Initial(int size)
        {
            return 5 * size / 11;
        }

        public int Decrease(int current)
        {
            if (current > 2)
                return 5 * current / 11;
            else if (current > 1)
                return 1;
            else
                return 0;
        }
    }
}
