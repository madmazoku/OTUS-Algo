using System;
using System.Collections.Generic;
using System.Text;

namespace lesson._09.cs
{
    class Utils
    {
        static public void PrintNodes(string text, SimpleNode root)
        {
            Console.Write($"{text}:");
            PrintNodes(root);
            Console.WriteLine("");
        }

        static void PrintNodes(SimpleNode node)
        {
            if (node == null)
                return;
            PrintNodes(node.left);
            Console.Write($" {node.x};");
            PrintNodes(node.right);
        }

        static public void Shuffle(int[] array)
        {
            Random rand = new Random();
            for (int i = 1; i < array.Length; ++i)
            {
                int j = rand.Next(i, array.Length);
                int t = array[i - 1];
                array[i - 1] = array[j];
                array[j] = t;
            }
        }


    }
}
