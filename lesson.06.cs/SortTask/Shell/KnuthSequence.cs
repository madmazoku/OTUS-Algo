namespace lesson._06.cs
{
    class KnuthSequence : IShellSequence
    {
        public string Name() { return "Knuth"; }

        public int Initial(int size)
        {
            int sizeDiv3 = size / 3;
            int current = 1;
            while (current < sizeDiv3)
                current = 3 * current + 1;
            return current;
        }

        public int Decrease(int current)
        {
            if (current >= 3)
                return current - current / 3;
            else if (current > 1)
                return 1;
            else
                return 0;
        }
    }
}
