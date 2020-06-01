using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace lesson._08.cs
{
    class FileMemoryAccessor : IMemoryAcessor
    {
        FileInfo file;

        MemoryMappedFile mmf;
        MemoryMappedViewAccessor mmva;
        long size;

        public FileMemoryAccessor(FileInfo file)
        {
            this.file = file;

            mmf = MemoryMappedFile.CreateFromFile(file.FullName);
            mmva = mmf.CreateViewAccessor();
            size = file.Length;
        }

        public FileMemoryAccessor(long size)
        {
            mmf = MemoryMappedFile.CreateNew(null, size);
            mmva = mmf.CreateViewAccessor();
            this.size = size;
        }

        ~FileMemoryAccessor()
        {
            mmva.Flush();
            mmva.Dispose();
            mmf.Dispose();
        }

        public long Size()
        {
            return size;
        }

        public IMemoryAcessor CloneAUX()
        {
            return new FileMemoryAccessor(size);
        }

        public UInt16 Read(long index)
        {
            return mmva.ReadUInt16(index * sizeof(UInt16));
        }

        public void Write(long index, UInt16 value)
        {
            mmva.Write(index * sizeof(UInt16), value);
        }

        public void Swap(long left, long right, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (left == right)
                return;

            UInt16 t = mmva.ReadUInt16(left * sizeof(UInt16));
            mmva.Write(left * sizeof(UInt16), mmva.ReadUInt16(right * sizeof(UInt16)));
            mmva.Write(right * sizeof(UInt16), t);
        }
    }
}
