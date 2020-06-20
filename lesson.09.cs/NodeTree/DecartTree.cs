using System;
using System.Collections.Generic;

namespace lesson._09.cs
{
    class DecartTree : INodeTree
    {
        class Node
        {
            public int x;
            public int y;

            public Node left;
            public Node right;

            public Node(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        Random rand;
        Node root;

        public DecartTree()
        {
            root = null;
            rand = new Random();
        }

        DecartTree(Node root, Random rand)
        {
            this.root = root;
            this.rand = rand;
        }

        public string Name()
        {
            return "Decart";
        }
        public int[] GetArray()
        {
            List<int> list = new List<int>();
            FillList(root, list);
            return list.ToArray();
        }
        public INodeTree Clone()
        {
            return new DecartTree(CloneNode(root), rand);
        }

        public void Insert(int x)
        {
            (Node leftNode, Node rightNode, _) = SplitNode(root, x);
            root = MergeNode(MergeNode(leftNode, new Node(x, rand.Next())), rightNode);
        }
        public bool Find(int x)
        {
            return FindNode(root, x) != null;
        }
        public void Remove(int x)
        {
            (Node leftNode, Node rightNode, _) = SplitNode(root, x);
            root = MergeNode(leftNode, rightNode);
        }

        Node FindNode(Node node, int x)
        {
            if (node == null || node.x == x)
                return node;
            if (x < node.x)
                return FindNode(node.left, x);
            else
                return FindNode(node.right, x);
        }

        (Node, Node, Node) SplitNode(Node node, int x)
        {
            if (node == null)
                return (null, null, null);

            Node equalNode;
            Node anotherNode;
            if (x < node.x)
            {
                (anotherNode, node.left, equalNode) = SplitNode(node.left, x);
                return (anotherNode, node, equalNode);
            }
            else if (x > node.x)
            {
                (node.right, anotherNode, equalNode) = SplitNode(node.right, x);
                return (node, anotherNode, equalNode);
            }
            else
            {
                Node leftNode = node.left;
                Node rightNode = node.right;
                node.left = null;
                node.right = null;
                return (leftNode, rightNode, node);
            }

        }

        Node MergeNode(Node leftNode, Node rightNode)
        {
            if (leftNode == null)
                return rightNode;
            if (rightNode == null)
                return leftNode;

            if (leftNode.y > rightNode.y)
            {
                leftNode.right = MergeNode(leftNode.right, rightNode);
                return leftNode;
            }
            else
            {
                rightNode.left = MergeNode(leftNode, rightNode.left);
                return rightNode;
            }
        }

        void FillList(Node node, List<int> list)
        {
            if (node == null)
                return;
            FillList(node.left, list);
            list.Add(node.x);
            FillList(node.right, list);
        }

        Node CloneNode(Node node)
        {
            if (node == null)
                return null;

            Node newNode = new Node(node.x, node.y);
            newNode.left = CloneNode(node.left);
            newNode.right = CloneNode(node.right);

            return newNode;
        }
    }
}
