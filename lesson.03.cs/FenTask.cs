using lesson._02.cs;

namespace lesson._03.cs
{
    class FENTask : ITask
    {
        public string Name() { return "FEN нотация"; }

        string input;
        ulong[] positions;

        public void Prepare(string[] data)
        {
            input = data[0];
        }

        public bool Result(string[] expect)
        {
            for (int i = 0; i < 12; ++i)
            {
                ulong position = positions[i];
                ulong expectPosition = ulong.Parse(expect[i]);
                if (position != expectPosition)
                    return false;
            }
            return true;
        }

        public void Run()
        {
            positions = Utils.ParseFEN(input);
        }

    }
}
