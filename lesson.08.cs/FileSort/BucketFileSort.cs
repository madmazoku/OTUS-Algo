using System;
using System.IO;
using System.Threading;

namespace lesson._08.cs
{
    class BucketFileSort : IFileSort
    {
        long bucketSize;

        public BucketFileSort(long bucketSize)
        {
            this.bucketSize = bucketSize;
        }

        public string Name() { return $"Bucket.{bucketSize}"; }
        public void Sort(FileInfo fileSource, FileInfo fileDestination, CancellationToken token)
        {
            BucketSort(fileSource, fileDestination, bucketSize, token);
        }

        class Node
        {
            public UInt16 value;
            public Node next;

            public Node(UInt16 value, Node next)
            {
                this.value = value;
                this.next = next;
            }
        }

        static void BucketSort(FileInfo fileSource, FileInfo fileDestination, long bucketSize, CancellationToken token)
        {
            long arraySize = fileSource.Length / sizeof(UInt16);
            long bucketCount = arraySize / bucketSize;
            Node[] buckets = new Node[bucketCount];
            byte[] buffer = new byte[sizeof(UInt16)];
            long maxValue = 1L + UInt16.MaxValue;

            FileStream streamSource = fileSource.OpenRead();
            while (streamSource.Read(buffer) == sizeof(UInt16))
            {
                token.ThrowIfCancellationRequested();

                UInt16 value = BitConverter.ToUInt16(buffer);
                long bucketIndex = bucketCount * value / maxValue;

                Node node = buckets[bucketIndex];
                if (node == null)
                    buckets[bucketIndex] = new Node(value, null);
                else
                {
                    Node prevNode = null;
                    while (node != null && node.value < value)
                    {
                        prevNode = node;
                        node = node.next;
                    }
                    if (prevNode == null)
                        buckets[bucketIndex] = new Node(value, node);
                    else
                        prevNode.next = new Node(value, node);
                }
            }
            streamSource.Close();

            FileStream streamDestination = fileDestination.OpenWrite();
            for (long bucketIndex = 0; bucketIndex < bucketCount; ++bucketIndex)
                for (Node node = buckets[bucketIndex]; node != null; node = node.next)
                {
                    token.ThrowIfCancellationRequested();
                    BitConverter.TryWriteBytes(buffer, node.value);
                    streamDestination.Write(buffer);
                }
        }
    }
}
