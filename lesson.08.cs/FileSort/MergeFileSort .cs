using System;
using System.Collections.Generic;
using System.IO;

namespace lesson._08.cs
{
    class MergeFileSort : IFileSort
    {
        IMASort maSort;
        int divider;

        public MergeFileSort(IMASort maSort, int divider)
        {
            this.maSort = maSort;
            this.divider = divider;
        }

        public string Name() { return $"Merge.{divider}.{maSort.Name()}"; }

        public void Sort(FileInfo fileSource, FileInfo fileDestination)
        {
            long arraySize = fileSource.Length / sizeof(UInt16);
            long stepLong = arraySize / divider;

            if (stepLong > int.MaxValue)
                throw new Exception("Too small divider");
            int step = (int)stepLong;

            Queue<FileInfo> fileParts = SortParts(fileSource, fileDestination, step, maSort);
            MergeParts(fileParts, fileDestination);
        }

        private Queue<FileInfo> SortParts(FileInfo fileSource, FileInfo fileDestination, int step, IMASort maSort)
        {
            string tmpFileName = Path.GetFileNameWithoutExtension(fileDestination.Name);
            int bufferSize = step * sizeof(UInt16);

            byte[] buffer = new byte[bufferSize];
            UInt16[] array = new UInt16[step];
            IMemoryAcessor ma = new ArrayMemoryAccessor(array);

            FileStream streamSource = fileSource.OpenRead();
            Queue<FileInfo> fileParts = new Queue<FileInfo>();

            while (true)
            {
                int readBytes = streamSource.Read(buffer);

                if (readBytes > 0)
                {
                    Buffer.BlockCopy(buffer, 0, array, 0, readBytes);
                    maSort.Sort(ma, 0, readBytes / sizeof(UInt16));
                    Buffer.BlockCopy(array, 0, buffer, 0, readBytes);

                    FileInfo filePart = new FileInfo(Path.Combine(fileDestination.DirectoryName, tmpFileName + $".partial.{fileParts.Count}" + fileDestination.Extension));
                    FileStream streamPart = filePart.OpenWrite();
                    streamPart.Write(buffer, 0, readBytes);
                    streamPart.Close();

                    fileParts.Enqueue(filePart);
                }

                if (readBytes < bufferSize)
                    break;
            }

            streamSource.Close();

            return fileParts;
        }

        private void MergeParts(Queue<FileInfo> fileParts, FileInfo fileDestination)
        {
            string tmpFileName = Path.GetFileNameWithoutExtension(fileDestination.Name);
            byte[] buffer1 = new byte[sizeof(UInt16)];
            byte[] buffer2 = new byte[sizeof(UInt16)];

            int mergeNum = 0;
            while (fileParts.Count > 1)
            {
                FileInfo filePart1 = fileParts.Dequeue();
                FileInfo filePart2 = fileParts.Dequeue();

                FileStream streamPart1 = filePart1.OpenRead();
                FileStream streamPart2 = filePart2.OpenRead();

                FileInfo fileMerged = new FileInfo(Path.Combine(fileDestination.DirectoryName, tmpFileName + $".merged.{mergeNum}" + fileDestination.Extension));
                FileStream streamMerged = fileMerged.OpenWrite();

                UInt16 valuePart1 = 0;
                bool hasValuePart1 = false;
                UInt16 valuePart2 = 0;
                bool hasValuePart2 = false;

                while (true)
                {
                    if (!hasValuePart1)
                    {
                        hasValuePart1 = streamPart1.Read(buffer1) == sizeof(UInt16);
                        if (hasValuePart1)
                            valuePart1 = BitConverter.ToUInt16(buffer1);
                    }

                    if (!hasValuePart2)
                    {
                        hasValuePart2 = streamPart2.Read(buffer2) == sizeof(UInt16);
                        if (hasValuePart2)
                            valuePart2 = BitConverter.ToUInt16(buffer2);
                    }

                    if (hasValuePart1 && hasValuePart2)
                        if (valuePart1 < valuePart2)
                        {
                            streamMerged.Write(buffer1);
                            hasValuePart1 = false;
                        }
                        else
                        {
                            streamMerged.Write(buffer2);
                            hasValuePart2 = false;
                        }
                    else
                        break;

                }

                if (hasValuePart1)
                {
                    streamMerged.Write(buffer1);
                    while (streamPart1.Read(buffer1) == sizeof(UInt16))
                        streamMerged.Write(buffer1);
                }
                else if (hasValuePart2)
                {
                    streamMerged.Write(buffer2);
                    while (streamPart2.Read(buffer2) == sizeof(UInt16))
                        streamMerged.Write(buffer2);
                }

                streamPart1.Close();
                streamPart2.Close();

                streamMerged.Close();

                filePart1.Delete();
                filePart2.Delete();

                fileParts.Enqueue(fileMerged);

                ++mergeNum;
            }

            FileInfo fileLastMerged = fileParts.Dequeue();
            Directory.Move(fileLastMerged.FullName, fileDestination.FullName);
            fileDestination.Refresh();
        }

    }
}
