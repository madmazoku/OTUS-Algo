using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace lesson._08.cs
{
    class Utils
    {
        static public UInt16[] LoadArray(FileInfo file)
        {
            UInt16[] array = new UInt16[file.Length / sizeof(UInt16)];
            byte[] buffer = new byte[sizeof(UInt16)];

            int index = 0;
            FileStream stream = file.OpenRead();
            while (stream.Read(buffer) != 0)
                array[index++] = BitConverter.ToUInt16(buffer);
            stream.Close();

            return array;
        }

        static public UInt16[] LoadArray(MemoryMappedViewAccessor mmva, long size)
        {
            UInt16[] array = new UInt16[size];
            for (int index = 0; index < array.Length; ++index)
                array[index] = mmva.ReadUInt16(index * sizeof(UInt16));
            return array;
        }

        static public void Swap(UInt16[] array, int left, int right)
        {
            if (left == right)
                return;

            UInt16 t = array[left];
            array[left] = array[right];
            array[right] = t;
        }

        static public void Swap(MemoryMappedViewAccessor mmva, long left, long right)
        {
            if (left == right)
                return;

            UInt16 t = mmva.ReadUInt16(left * sizeof(UInt16));
            mmva.Write(left * sizeof(UInt16), mmva.ReadUInt16(right * sizeof(UInt16)));
            mmva.Write(right * sizeof(UInt16), t);
        }

        static public void Copy(IMemoryAcessor source, long sourceIndex, IMemoryAcessor destination, long destinationIndex, long length, CancellationToken token)
        {
            for (long index = 0; index < length; ++index)
            {
                token.ThrowIfCancellationRequested();
                destination.Write(destinationIndex + index, source.Read(sourceIndex + index));
            }
        }

    }
}
