using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace lesson._08.cs
{
    class MMFFileSort : IFileSort
    {
        IMMFSort mmfSort;

        public MMFFileSort(IMMFSort mmfSort)
        {
            this.mmfSort = mmfSort;
        }

        public string Name() { return $"MMF.{mmfSort.Name()}"; }

        public void Sort(FileInfo fileSource, FileInfo fileDestination)
        {
            File.Copy(fileSource.FullName, fileDestination.FullName);
            fileDestination.Refresh();
            MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(fileDestination.FullName);
            MemoryMappedViewAccessor mmva = mmf.CreateViewAccessor();

            mmfSort.Sort(mmva, 0, fileDestination.Length / sizeof(UInt16));

            mmva.Flush();
            mmva.Dispose();
            mmf.Dispose();
        }

    }
}
