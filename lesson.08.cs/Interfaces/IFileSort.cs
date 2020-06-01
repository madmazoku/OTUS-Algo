using System.IO;
using System.Threading;

namespace lesson._08.cs
{
    interface IFileSort
    {
        public string Name();
        public void Sort(FileInfo fileSource, FileInfo fileDestination, CancellationToken token);
    }
}
