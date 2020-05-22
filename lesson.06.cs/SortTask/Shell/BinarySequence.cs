namespace lesson._06.cs
{
    class BinarySequence : IShellSequence
    {
        public string Name() { return "Binary"; }

        public int Initial(int size)
        {
            return size >> 1;
        }

        public int Decrease(int current)
        {
            return current >> 1;
        }
    }
}
