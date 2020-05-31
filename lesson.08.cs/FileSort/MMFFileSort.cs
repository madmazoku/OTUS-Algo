﻿using System;
using System.IO;

namespace lesson._08.cs
{
    class MMFFileSort : IFileSort
    {
        IMASort maSort;

        public MMFFileSort(IMASort maSort)
        {
            this.maSort = maSort;
        }

        public string Name() { return $"MMF.{maSort.Name()}"; }

        public void Sort(FileInfo fileSource, FileInfo fileDestination)
        {
            File.Copy(fileSource.FullName, fileDestination.FullName);
            fileDestination.Refresh();

            IMemoryAcessor ma = new FileMemoryAccessor(fileDestination);
            maSort.Sort(ma, 0, fileDestination.Length / sizeof(UInt16));
        }

    }
}
