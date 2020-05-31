namespace lesson._08.cs
{
    interface IMASort
    {
        public string Name();
        public void Sort(IMemoryAcessor ma, long start, long end);
    }
}
