using System;

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

        static void TestSimple()
        {
            for (int t = 0; t < 2; ++t)
            {
                int[] inserts = Utils.MakeIndexArray(20);
                Utils.Shuffle(inserts);

                //SimpleTree nodeTree = new SimpleTree();
                AVLTree nodeTree = new AVLTree();

                Utils.PrintArray("Insertions", inserts);
                for (int i = 0; i < inserts.Length; ++i)
                {
                    nodeTree.Insert(inserts[i]);
                    int[] check = nodeTree.GetArray();
                    if (check.Length != i + 1)
                        Console.WriteLine("\tSize mismatch");
                    if (!Utils.IsOrdered(check))
                        Console.WriteLine("\tNot ordered");
                    if (!Utils.IsHaveElement(check, inserts[i]))
                        Console.WriteLine("\tInserted element not found");
                }
                Utils.PrintArray("Overall", nodeTree.GetArray());

                int[] removes = Utils.Sample(inserts, 10);
                Utils.PrintArray("Removes", removes);
                for (int i = 0; i < removes.Length; ++i)
                {
                    nodeTree.Remove(inserts[i]);
                    int[] check = nodeTree.GetArray();
                    if (check.Length != inserts.Length - (i + 1))
                        Console.WriteLine("\tSize mismatch");
                    if (!Utils.IsOrdered(check))
                        Console.WriteLine("\tNot ordered");
                    if (Utils.IsHaveElement(check, inserts[i]))
                        Console.WriteLine("\tRemoved element found");
                }
                Utils.PrintArray("Overall", nodeTree.GetArray());

                Console.WriteLine("");

                //nodeTree.Print("start");

                //nodeTree.Remove(5);

                //nodeTree.Print("remove 5");

            }

            Console.WriteLine("Hello World!");
        }

        static void TestArrayClone()
        {
            //SimpleTree nodeTree = new SimpleTree();
            AVLTree nodeTree = new AVLTree();

            int[] inserts = Utils.MakeIndexArray(20);
            Utils.Shuffle(inserts);
            Utils.PrintArray("Array", inserts);
            for (int i = 0; i < inserts.Length; ++i)
                nodeTree.Insert(inserts[i]);
            Utils.PrintArray("Inserted", nodeTree.GetArray());

            INodeTree nodeTreeClone = nodeTree.Clone();
            Utils.PrintArray("Cloned", nodeTreeClone.GetArray());

        }

        static void NodeTreeTest()
        {
            Tester tester = new Tester("Trees");
            tester.Add(new SimpleTree());
            tester.Add(new AVLTree());
            tester.Add(new RandomTestCase(100));
            tester.Add(new OrderedTestCase(100, false));
            tester.Add(new OrderedTestCase(100, true));
            tester.Add(new RandomTestCase(1000));
            tester.Add(new OrderedTestCase(1000, false));
            tester.Add(new OrderedTestCase(1000, true));
            tester.Add(new RandomTestCase(10000));
            tester.Add(new OrderedTestCase(10000, false));
            tester.Add(new OrderedTestCase(10000, true));
            tester.Add(new RandomTestCase(100000));
            tester.Add(new OrderedTestCase(100000, false));
            tester.Add(new OrderedTestCase(100000, true));
            tester.Add(new RandomTestCase(1000000));
            tester.Add(new RandomTestCase(10000000));
            tester.RunTests();
        }

        static void Main(string[] args)
        {
            TestSimple();
            //TestArrayClone();
            //NodeTreeTest();
        }

    }
}
