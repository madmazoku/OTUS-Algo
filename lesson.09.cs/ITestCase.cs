namespace lesson._09.cs
{
    interface ITestCase
    {
        public string Name();

        public void Prepare();

        public int[] GetInsertArray();
        public int[] GetFindArray();
        public int[] GetRemoveArray();
    }
}
