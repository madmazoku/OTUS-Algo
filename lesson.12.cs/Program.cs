using System;
using System.Diagnostics;

namespace lesson._12.cs
{
    class Program
    {

        static void TestHashTable(IHashTable hashTable)
        {
            // Check Insert
            Debug.Assert(hashTable.Insert("a", "a") == null);
            Debug.Assert(hashTable.Insert("b", "b") == null);
            Debug.Assert(hashTable.Insert("c", "c") == null);
            Debug.Assert(hashTable.Insert("abba", "abba") == null);
            Debug.Assert(hashTable.Insert("abracadabra", "abracadabra") == null);
            Debug.Assert(hashTable.Insert("baab", "baab") == null);
            Debug.Assert(hashTable.Insert("pika", "pika") == null);
            Debug.Assert(hashTable.Insert("pica", "pica") == null);
            Debug.Assert(hashTable.Insert("1234567890", "1234567890") == null);
            Debug.Assert(hashTable.Insert("0987654321", "0987654321") == null);

            Debug.Assert(hashTable.Insert("a", "a2") == "a");
            Debug.Assert(hashTable.Insert("b", "b2") == "b");
            Debug.Assert(hashTable.Insert("c", "c2") == "c");

            // Check Find
            Debug.Assert(hashTable.Find("a") == "a2");
            Debug.Assert(hashTable.Find("b") == "b2");
            Debug.Assert(hashTable.Find("c") == "c2");
            Debug.Assert(hashTable.Find("abba") == "abba");
            Debug.Assert(hashTable.Find("abracadabra") == "abracadabra");
            Debug.Assert(hashTable.Find("baab") == "baab");
            Debug.Assert(hashTable.Find("pika") == "pika");
            Debug.Assert(hashTable.Find("pica") == "pica");
            Debug.Assert(hashTable.Find("1234567890") == "1234567890");
            Debug.Assert(hashTable.Find("0987654321") == "0987654321");

            Debug.Assert(hashTable.Find("aa") == null);
            Debug.Assert(hashTable.Find("bb") == null);
            Debug.Assert(hashTable.Find("cc") == null);

            // Check Remove
            Debug.Assert(hashTable.Remove("aa") == null);
            Debug.Assert(hashTable.Remove("bb") == null);
            Debug.Assert(hashTable.Remove("cc") == null);

            Debug.Assert(hashTable.Remove("a") == "a2");
            Debug.Assert(hashTable.Remove("b") == "b2");
            Debug.Assert(hashTable.Remove("c") == "c2");
            Debug.Assert(hashTable.Remove("abba") == "abba");
            Debug.Assert(hashTable.Remove("abracadabra") == "abracadabra");
            Debug.Assert(hashTable.Remove("baab") == "baab");
            Debug.Assert(hashTable.Remove("pika") == "pika");
            Debug.Assert(hashTable.Remove("pica") == "pica");
            Debug.Assert(hashTable.Remove("1234567890") == "1234567890");
            Debug.Assert(hashTable.Remove("0987654321") == "0987654321");

            Debug.Assert(hashTable.Remove("aa") == null);
            Debug.Assert(hashTable.Remove("bb") == null);
            Debug.Assert(hashTable.Remove("cc") == null);

            // Check Find after Remove
            Debug.Assert(hashTable.Find("a") == null);
            Debug.Assert(hashTable.Find("b") == null);
            Debug.Assert(hashTable.Find("c") == null);

            Debug.Assert(hashTable.Find("abba") == null);
            Debug.Assert(hashTable.Find("abracadabra") == null);
            Debug.Assert(hashTable.Find("baab") == null);
            Debug.Assert(hashTable.Find("pika") == null);
            Debug.Assert(hashTable.Find("pica") == null);
            Debug.Assert(hashTable.Find("1234567890") == null);
            Debug.Assert(hashTable.Find("0987654321") == null);

            // Check Insert after Remove
            Debug.Assert(hashTable.Insert("a", "a") == null);
            Debug.Assert(hashTable.Insert("b", "b") == null);
            Debug.Assert(hashTable.Insert("c", "c") == null);
            Debug.Assert(hashTable.Insert("abba", "abba") == null);
            Debug.Assert(hashTable.Insert("abracadabra", "abracadabra") == null);
            Debug.Assert(hashTable.Insert("baab", "baab") == null);
            Debug.Assert(hashTable.Insert("pika", "pika") == null);
            Debug.Assert(hashTable.Insert("pica", "pica") == null);
            Debug.Assert(hashTable.Insert("1234567890", "1234567890") == null);
            Debug.Assert(hashTable.Insert("0987654321", "a0987654321") == null);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            TestHashTable(new LinkedHashTable());
            TestHashTable(new OpenHashTable());

        }
    }
}
