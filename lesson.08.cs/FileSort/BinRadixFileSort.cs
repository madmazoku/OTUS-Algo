using System;
using System.IO;
using System.Threading;

namespace lesson._08.cs
{
    class BinRadixFileSort : IFileSort
    {
        static UInt16 bits = 6;

        public string Name() { return "BinRadix"; }

        public void Sort(FileInfo fileSource, FileInfo fileDestination, CancellationToken token)
        {
            UInt16 radix = (UInt16)(1U << bits);
            UInt16 mask = (UInt16)(radix - 1);

            string tmpFileName = Path.GetFileNameWithoutExtension(fileDestination.Name);
            FileInfo fileTmp = new FileInfo(Path.Combine(fileSource.DirectoryName, tmpFileName + $".tmp" + fileDestination.Extension));
            FileInfo[] fileTmps = new FileInfo[radix];
            FileStream[] streamTmps = new FileStream[radix];
            for (int index = 0; index < radix; ++index)
                fileTmps[index] = new FileInfo(Path.Combine(fileSource.DirectoryName, tmpFileName + $".{index}" + fileDestination.Extension));

            File.Copy(fileSource.FullName, fileDestination.FullName);
            fileDestination.Refresh();

            UInt16 shift = 0;
            byte[] buffer = new byte[sizeof(UInt16)];
            bool hasAnotherDigit = true;
            while (hasAnotherDigit)
            {
                if (fileTmp.Exists)
                    fileTmp.Delete();
                File.Move(fileDestination.FullName, fileTmp.FullName);
                fileTmp.Refresh();

                for (int index = 0; index < radix; ++index)
                    streamTmps[index] = fileTmps[index].OpenWrite();

                hasAnotherDigit = false;
                FileStream streamTmp = fileTmp.OpenRead();
                while (streamTmp.Read(buffer) != 0)
                {
                    token.ThrowIfCancellationRequested();

                    UInt16 value = BitConverter.ToUInt16(buffer);
                    value >>= shift;
                    hasAnotherDigit = hasAnotherDigit || (value >= radix);
                    streamTmps[value & mask].Write(buffer);
                }
                streamTmp.Close();

                FileStream streamDestination = fileDestination.OpenWrite();
                for (int index = 0; index < radix; ++index)
                {
                    streamTmps[index].Close();

                    streamTmps[index] = fileTmps[index].OpenRead();
                    while (streamTmps[index].Read(buffer) != 0)
                    {
                        token.ThrowIfCancellationRequested();
                        streamDestination.Write(buffer);
                    }
                    streamTmps[index].Close();

                    fileTmps[index].Delete();
                }
                streamDestination.Close();

                shift += bits;
            }
            fileTmp.Delete();
        }
    }
}
