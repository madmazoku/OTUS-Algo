using System;

namespace lesson._21.cs
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Конечный автомат");

            string text = "ABABABC";
            string pattern = "ABABC";
            //string pattern = "AABAAABAAAB";

            Console.WriteLine($"FSM Search: \"{text}\" has \"{pattern}\" from {(new SearchFSM(pattern)).Do(text)} index");
            Console.WriteLine($"Pi Search:  \"{text}\" has \"{pattern}\" from {(new SearchPi(pattern)).Do(text)} index");
        }
    }
}
