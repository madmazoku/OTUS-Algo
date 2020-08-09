namespace lesson._23.cs
{
    class RLESimpleCodec : ICodec
    {
        public void Encode(IData input, IData output)
        {
            int count = 1;
            int prev = input.ReadByte();

            while (prev != -1)
            {
                int next = input.ReadByte();
                if (next != prev || count == byte.MaxValue)
                {
                    output.WriteByte((byte)count);
                    output.WriteByte((byte)prev);
                    count = 0;
                    prev = next;
                }
                ++count;
            }
            output.Flush();
        }

        public void Decode(IData input, IData output)
        {
            int count;
            while ((count = input.ReadByte()) != -1)
            {
                int value = input.ReadByte();
                while (count-- > 0)
                    output.WriteByte((byte)value);
            }
            output.Flush();
        }
    }
}
