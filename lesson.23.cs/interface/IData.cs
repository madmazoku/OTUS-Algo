namespace lesson._23.cs
{
    interface IData
    {
        public int ReadByte();
        public int Read(byte[] array, int offset, int length);

        public void WriteByte(byte value);
        public void Write(byte[] array, int offset, int length);

        public void Flush();
    }
}
