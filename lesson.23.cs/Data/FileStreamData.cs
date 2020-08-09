using System.IO;

namespace lesson._23.cs
{
    class FileStreamData : IData
    {
        FileStream fs;

        public FileStreamData(FileStream fs)
        {
            this.fs = fs;
        }

        public int ReadByte()
        {
            return fs.ReadByte();
        }
        public int Read(byte[] array, int offset, int length)
        {
            return fs.Read(array, offset, length);
        }

        public void WriteByte(byte value)
        {
            fs.WriteByte(value);
        }
        public void Write(byte[] array, int offset, int length)
        {
            fs.Write(array, offset, length);
        }

        public void Flush()
        {
            fs.Flush();
        }
    }

}
