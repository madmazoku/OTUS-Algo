using System.IO;

namespace lesson._08.cs
{
    interface IFileSort
    {
        public string Name();
        public void Sort(FileInfo fileSource, FileInfo fileDestination);
    }
}
