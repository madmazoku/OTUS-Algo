namespace lesson._02.cs
{
    interface ITask
    {
        public string Name();

        public void Prepare(string[] data);
        public bool Result(string[] expect);

        public void Run();
    }
}
