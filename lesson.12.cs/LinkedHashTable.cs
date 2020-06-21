using System;

namespace lesson._12.cs
{
    class LinkedHashTable : IHashTable
    {
        Func<string, int, int> hash;

        class Node
        {
            public string key;
            public string value;
            public Node next;

            public Node(string key, string value, Node next)
            {
                this.key = key;
                this.value = value;
                this.next = next;
            }
        };

        Node[] array;
        int size;

        static readonly double loadFactor = 0.7;
        static readonly int startArraySize = 10;

        static int hashBase(string key, int range)
        {
            UInt16 hash = (UInt16)(0xaaaa ^ range);
            foreach (char c in key)
                hash = (UInt16)(((hash >> 4) | (hash << 12)) ^ c);
            return hash % range;
        }

        public LinkedHashTable()
        {
            this.hash = hashBase;
            array = new Node[startArraySize];
        }

        public LinkedHashTable(Func<string, int, int> hash)
        {
            this.hash = hash;
            array = new Node[startArraySize];
        }

        public string Insert(string key, string value)
        {
            int index = hash(key, array.Length);

            ref Node node = ref array[index];
            while (node != null)
            {
                if (node.key == key)
                {
                    string oldValue = node.value;
                    node.value = value;
                    return oldValue;
                }
                node = ref node.next;
            }

            node = new Node(key, value, null);
            SizeIncrement();
            return null;
        }
        public string Find(string key)
        {
            int index = hash(key, array.Length);

            Node node = array[index];
            while (node != null)
            {
                if (node.key == key)
                    return node.value;
                node = node.next;
            }
            return null;
        }
        public string Remove(string key)
        {
            int index = hash(key, array.Length);

            ref Node node = ref array[index];
            while (node != null)
            {
                if (node.key == key)
                {
                    SizeDecrement();
                    string value = node.value;
                    node = node.next;
                    return value;
                }
                node = ref node.next;
            }
            return null;
        }

        void SizeIncrement()
        {
            if (++size < loadFactor * array.Length)
                return;

            Node[] newArray = new Node[array.Length << 1];
            for (int index = 0; index < array.Length; ++index)
            {
                ref Node node = ref array[index];
                while (node != null)
                {
                    int newIndex = hash(node.key, newArray.Length);
                    ref Node newNode = ref newArray[newIndex];
                    while (newNode != null)
                        newNode = ref newNode.next;
                    newNode = node;
                    node = node.next;
                    newNode.next = null;
                }
            }
            array = newArray;
        }

        void SizeDecrement()
        {
            --size;
        }

    }
}
