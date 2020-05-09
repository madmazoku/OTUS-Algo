namespace lesson._02.cs
{
    interface ITask
    {
        public string Name();

        public void Prepare(string[] data);
        public void Run();
        public bool Result(string expected);
    }
}
