using System;

namespace lesson._12.cs
{
    class OpenHashTable : IHashTable
    {
        Func<string, int, int> hash;

        class Node
        {
            public string key;
            public string value;
            public bool isDeleted;

            public Node(string key, string value)
            {
                this.key = key;
                this.value = value;
            }
        }

        Node[] array;
        int size;

        static readonly int c1 = 7;
        static readonly int c2 = 3;
        static readonly int M = 11;

        static readonly double loadFactor = 0.7;
        static readonly int startArraySize = 10;

        static int hashBase(string key, int range)
        {
            UInt16 hash = (UInt16)(0xaaaa ^ range);
            foreach (char c in key)
                hash = (UInt16)((((hash >> 4) & 0xff) | (hash << 12)) ^ c);
            return hash % range;
        }

        static int hashExt(int hashIndex, int offset, int range)
        {
            return (hashIndex + (c1 * offset + c2 * offset * offset) % M) % range;
        }

        public OpenHashTable()
        {
            this.hash = hashBase;
            array = new Node[startArraySize];
        }

        public OpenHashTable(Func<string, int, int> hash)
        {
            this.hash = hash;
            array = new Node[startArraySize];
        }

        public string Insert(string key, string value)
        {
            int hashIndex = hash(key, array.Length);
            int? firstRemovedIndex = null;
            for (int offset = 0; offset < M; ++offset)
            {
                int index = hashExt(hashIndex, offset, array.Length);
                if (array[index] == null)
                {
                    array[index] = new Node(key, value);
                    IncrementSize();
                    return null;
                }
                if (array[index].isDeleted)
                {
                    if (firstRemovedIndex == null)
                        firstRemovedIndex = index;
                }
                else
                {
                    if (array[index].key == key)
                    {
                        string oldValue = array[index].value;
                        array[index].value = value;
                        if (firstRemovedIndex != null)
                        {
                            Node node = array[index];
                            array[index] = array[(int)firstRemovedIndex];
                            array[(int)firstRemovedIndex] = node;
                        }
                        return oldValue;
                    }

                }
            }
            if (firstRemovedIndex != null)
            {
                array[(int)firstRemovedIndex] = new Node(key, value);
                return null;
            }
            throw new OverflowException();
        }

        public string Find(string key)
        {
            int hashIndex = hash(key, array.Length);
            for (int offset = 0; offset < M; ++offset)
            {
                int index = hashExt(hashIndex, offset, array.Length);
                if (array[index] == null)
                    return null;
                if (!array[index].isDeleted && array[index].key == key)
                    return array[index].value;
            }
            return null;
        }

        public string Remove(string key)
        {
            int hashIndex = hash(key, array.Length);
            for (int offset = 0; offset < M; ++offset)
            {
                int index = hashExt(hashIndex, offset, array.Length);
                if (array[index] == null)
                    return null;
                if (!array[index].isDeleted && array[index].key == key)
                {
                    DecrementSize();
                    array[index].isDeleted = true;
                    return array[index].value;
                }
            }
            return null;
        }

        void IncrementSize()
        {
            if (++size < loadFactor * array.Length)
                return;

            Node[] newArray = new Node[array.Length << 1];
            for (int index = 0; index < array.Length; ++index)
            {
                if (array[index] == null || array[index].isDeleted)
                    continue;

                int hashIndex = hash(array[index].key, newArray.Length);
                for (int offset = 0; offset <= M; ++offset)
                {
                    if (offset == M)
                        throw new OverflowException();

                    int newIndex = hashExt(hashIndex, offset, newArray.Length);
                    if (newArray[newIndex] == null)
                    {
                        newArray[newIndex] = array[index];
                        break;
                    }
                }
            }

            array = newArray;
        }

        void DecrementSize()
        {
            --size;
        }

    }
}
