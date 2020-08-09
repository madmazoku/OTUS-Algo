using System;

namespace lesson._23.cs
{
    class ArrayData : IData
    {
        public byte[] Data { get; private set; }
        public int Offset { get; private set; }

        public ArrayData()
        {
            Data = new byte[0];
            Offset = 0;
        }

        public void Print(string title)
        {
            Console.WriteLine($"{title}");
            Console.WriteLine($"\toffset: {Offset}");
            Console.WriteLine($"\tlength: {Data.Length}");
            if (Data.Length == 0)
                return;
            Console.Write($"\tdata: {Data[0],3}");
            for (int n = 1; n < Data.Length; ++n)
                Console.Write($"; {Data[n],3}");
            Console.WriteLine("");
        }
        public void Rewind()
        {
            Offset = 0;
        }
        public void Clear()
        {
            Data = new byte[0];
            Offset = 0;
        }
        void Resize(int size)
        {
            if (size <= Data.Length)
                return;
            byte[] data = new byte[size];
            Array.Copy(Data, data, Data.Length);
            Data = data;
        }

        public int ReadByte()
        {
            if (Offset < Data.Length)
                return Data[Offset++];
            else
                return -1;
        }
        public int Read(byte[] array, int offset, int length)
        {
            if (Offset + length > Data.Length)
                length = Data.Length - Offset;
            Array.Copy(Data, Offset, array, offset, length);
            Offset += length;
            return length;
        }

        public void WriteByte(byte value)
        {
            Resize(Offset + 1);
            Data[Offset++] = value;
        }
        public void Write(byte[] array, int offset, int length)
        {
            Resize(Offset + length);
            Array.Copy(array, offset, Data, Offset, length);
            Offset += length;
        }

        public void Flush()
        {

        }
    }

}
