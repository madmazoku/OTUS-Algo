using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lesson._09.cs
{
    class Program
    {
        class Node
        {
            public int key;
            public Node parent;
            public Node left;
            public Node right;

            public Node(int key, Node parent, Node left, Node right)
            {
                this.key = key;
                this.parent = parent;
                this.left = left;
                this.right = right;
            }

        }

        //class NodeTree
        //{
        //    Node root;

        //    public void Add(int key)
        //    {
        //        Add(new Node(key, null, null, null));
        //    }

        //    public void Add(Node node)
        //    {
        //        if (node == null)
        //            return;

        //        if (root == null)
        //        {
        //            node.parent = null;
        //            root = node;
        //            return;
        //        }

        //        Node currNode = root;
        //        while (true)
        //        {
        //            if (node.key < currNode.key)
        //            {
        //                if (currNode.left == null)
        //                {
        //                    node.parent = currNode;
        //                    currNode.left = node;
        //                    break;
        //                }
        //                else
        //                    currNode = currNode.left;
        //            }
        //            else
        //            {
        //                if (currNode.right == null)
        //                {
        //                    node.parent = currNode;
        //                    currNode.right = node;
        //                    break;
        //                }
        //                else
        //                    currNode = currNode.right;
        //            }
        //        }
        //    }

        //    public Node Find(int key)
        //    {
        //        Node currNode = root;
        //        while (true)
        //        {
        //            if (currNode == null || currNode.key == key)
        //                return currNode;
        //            if (key < currNode.key)
        //                currNode = currNode.left;
        //            else
        //                currNode = currNode.right;
        //        }
        //    }

        //    public void Remove(int key)
        //    {
        //        Node node = Find(key);
        //        if (node == null)
        //            return;
        //        if (node.parent == null)
        //            root = null;
        //        else
        //        {
        //            if (node.parent.left == node)
        //                node.parent.left = null;
        //            else
        //                node.parent.right = null;
        //        }
        //        Add(node.right);
        //        Add(node.left);
        //    }

        //    public void Print(string text)
        //    {
        //        Console.WriteLine(text);
        //        PrintLine(root);
        //        Console.WriteLine("\n");

        //        PrintPath(root, "!");
        //        Console.WriteLine("");

        //        Stack<string> treeLevels = new Stack<string>();
        //        BuildPrint(treeLevels, root, 0);
        //        while (treeLevels.Count > 0)
        //            Console.WriteLine(treeLevels.Pop());
        //        Console.WriteLine("");
        //    }

        //    public void PrintLine(Node node)
        //    {
        //        if (node == null)
        //            return;
        //        PrintLine(node.left);
        //        Console.Write($" {node.key};");
        //        PrintLine(node.right);
        //    }

        //    public void PrintPath(Node node, string path)
        //    {
        //        if (node == null)
        //            return;

        //        Console.WriteLine($"{path}: {node.key}");

        //        PrintPath(node.left, path + "l");
        //        PrintPath(node.right, path + "r");
        //    }

        //    int BuildPrint(Stack<string> treeLevels, Node node, int defaultIdent)
        //    {
        //        if (node == null)
        //            return 0;

        //        string level;
        //        if (treeLevels.Count > 0)
        //            level = treeLevels.Pop();
        //        else
        //            level = new String(' ', defaultIdent);

        //        int widthLeft = BuildPrint(treeLevels, node.left, level.Length);
        //        int widthRight = BuildPrint(treeLevels, node.right, level.Length + widthLeft);

        //        string value = $"({node.key, 4})";
        //        if (node.left != null && node.right != null)
        //            value = "<-" + value + "->";
        //        else if (node.left != null)
        //            value = "v" + value;
        //        else if (node.right != null)
        //            value = value + "v";

        //        int valueWidthLeft = (value.Length + 1) >> 1;
        //        int valueWidthRight = value.Length - valueWidthLeft;

        //        if (widthLeft < valueWidthLeft)
        //            widthLeft = valueWidthLeft + 1;
        //        if (widthRight < valueWidthRight)
        //            widthRight = valueWidthRight + 1;

        //        int identLeft = widthLeft - valueWidthLeft;
        //        int identRight = widthRight - valueWidthRight;
        //        int identHalfLeft = (identLeft + 1) >> 1;
        //        int identHalfRight = (identRight + 1) >> 1;

        //        StringBuilder sb = new StringBuilder(widthLeft + widthRight);
        //        sb.Append(level);
        //        sb.Append(new String(' ', identLeft));
        //        sb.Append(value);
        //        sb.Append(new String(' ', identRight));

        //        treeLevels.Push(sb.ToString());

        //        return widthLeft + widthRight;
        //    }
        //}

        static void Main(string[] args)
        {
            for (int t = 0; t < 10; ++t)
            {
                int[] keys = new int[20];
                for (int i = 0; i < keys.Length; ++i)
                    keys[i] = i;
            
                Utils.Shuffle(keys);

                SimpleNodeTree nodeTree = new SimpleNodeTree();

                for (int i = 0; i < keys.Length; ++i)
                    nodeTree.Insert(keys[i]);
                Utils.PrintNodes("inserts", nodeTree.Root());

                Random rand = new Random();
                int x = rand.Next(keys.Length);
                nodeTree.Remove(x);
                Utils.PrintNodes($"delete {x}", nodeTree.Root());

                //nodeTree.Print("start");

                //nodeTree.Remove(5);

                //nodeTree.Print("remove 5");

            }

            Console.WriteLine("Hello World!");
        }
    }
}
