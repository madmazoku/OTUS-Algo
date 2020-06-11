namespace lesson._09.cs
{
    interface INodeTree
    {
        public string Name();
        public int[] GetArray();
        public INodeTree Clone();

        public void Insert(int x);
        public bool Find(int x);
        public void Remove(int x);

    }
}
