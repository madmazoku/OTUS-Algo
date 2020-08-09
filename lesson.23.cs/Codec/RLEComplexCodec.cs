namespace lesson._23.cs
{
    class RLEComplexCodec : ICodec
    {
        public void Encode(IData input, IData output)
        {
            byte[] buffer = new byte[-sbyte.MinValue];
            int uniq = 0;
            int count = 1;
            int prev = input.ReadByte();

            while (prev != -1)
            {
                int next = input.ReadByte();
                if (next != prev)
                {
                    if (count > 1)
                    {
                        output.WriteByte((byte)count);
                        output.WriteByte((byte)prev);
                    }
                    else
                    {
                        buffer[uniq++] = (byte)prev;
                        if (uniq == -sbyte.MinValue || next == -1)
                        {
                            output.WriteByte((byte)(uniq + sbyte.MaxValue));
                            output.Write(buffer, 0, uniq);
                            uniq = 0;
                        }
                    }
                    count = 1;
                    prev = next;
                }
                else
                {
                    if (uniq > 0)
                    {
                        output.WriteByte((byte)(uniq + sbyte.MaxValue));
                        output.Write(buffer, 0, uniq);
                        uniq = 0;
                    }
                    if (count == sbyte.MaxValue)
                    {
                        output.WriteByte((byte)count);
                        output.WriteByte((byte)prev);
                        count = 0;
                    }
                    ++count;
                }
            }
            output.Flush();
        }

        public void Decode(IData input, IData output)
        {
            byte[] buffer = new byte[-sbyte.MinValue];
            int count;
            while ((count = input.ReadByte()) != -1)
                if (count <= sbyte.MaxValue)
                {
                    int value = input.ReadByte();
                    while (count-- > 0)
                        output.WriteByte((byte)value);
                }
                else
                {
                    count -= sbyte.MaxValue;
                    input.Read(buffer, 0, count);
                    output.Write(buffer, 0, count);
                }
            output.Flush();
        }
    }
}
