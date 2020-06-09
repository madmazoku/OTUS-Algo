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

        class NodeTree
        {
            Node root;

            public void Add(int key)
            {
                Add(new Node(key, null, null, null));
            }

            public void Add(Node node)
            {
                if (node == null)
                    return;

                if (root == null)
                {
                    node.parent = null;
                    root = node;
                    return;
                }

                Node currNode = root;
                while (true)
                {
                    if (node.key < currNode.key)
                    {
                        if (currNode.left == null)
                        {
                            node.parent = currNode;
                            currNode.left = node;
                            break;
                        }
                        else
                            currNode = currNode.left;
                    }
                    else
                    {
                        if (currNode.right == null)
                        {
                            node.parent = currNode;
                            currNode.right = node;
                            break;
                        }
                        else
                            currNode = currNode.right;
                    }
                }
            }

            public Node Find(int key)
            {
                Node currNode = root;
                while (true)
                {
                    if (currNode == null || currNode.key == key)
                        return currNode;
                    if (key < currNode.key)
                        currNode = currNode.left;
                    else
                        currNode = currNode.right;
                }
            }

            public void Remove(int key)
            {
                Node node = Find(key);
                if (node == null)
                    return;
                if (node.parent == null)
                    root = null;
                else
                {
                    if (node.parent.left == node)
                        node.parent.left = null;
                    else
                        node.parent.right = null;
                }
                Add(node.right);
                Add(node.left);
            }

            public void Print(string text)
            {
                Console.WriteLine(text);
                Print(root, " ");
                Console.WriteLine("");
            }

            void Print(Node node, string ident)
            {
                if (node == null)
                    return;

                Print(node.left, ident + " ");
                Console.WriteLine($"{ident}{node.key}");
                Print(node.right, ident + " ");
            }
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

        static void Main(string[] args)
        {
            int[] keys = new int[10];
            for (int i = 0; i < keys.Length; ++i)
                keys[i] = i;
            Shuffle(keys);

            NodeTree nodeTree = new NodeTree();

            for (int i = 0; i < keys.Length; ++i)
                nodeTree.Add(keys[i]);

            nodeTree.Print("start");

            nodeTree.Remove(5);

            nodeTree.Print("remove 5");

            Console.WriteLine("Hello World!");
        }
    }
}
