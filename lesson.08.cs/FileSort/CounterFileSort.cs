using System;
using System.IO;
using System.Threading;

namespace lesson._08.cs
{
    class CounterFileSort : IFileSort
    {
        public string Name() { return "Counter"; }

        public void Sort(FileInfo fileSource, FileInfo fileDestination, CancellationToken token)
        {
            long[] counts = new long[1L + UInt16.MaxValue];
            byte[] buffer = new byte[sizeof(UInt16)];

            FileStream fileStreamSource = fileSource.OpenRead();
            while (fileStreamSource.Read(buffer) != 0)
            {
                token.ThrowIfCancellationRequested();
                ++counts[BitConverter.ToUInt16(buffer)];
            }
            fileStreamSource.Close();

            FileStream fileStreamDestination = fileDestination.OpenWrite();
            for (long countIndex = 0; countIndex < counts.Length; ++countIndex)
            {
                BitConverter.TryWriteBytes(buffer, countIndex);
                while (counts[countIndex]-- > 0)
                {
                    token.ThrowIfCancellationRequested();
                    fileStreamDestination.Write(buffer);
                }
            }
            fileStreamDestination.Close();
        }
    }
}
