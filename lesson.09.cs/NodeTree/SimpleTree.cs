using System.Collections.Generic;

namespace lesson._09.cs
{
    class SimpleTree : INodeTree
    {
        class Node
        {
            public int x;
            public Node left;
            public Node right;

            public Node(int x) { this.x = x; }
        };

        Node root;

        public SimpleTree()
        {
            this.root = null;
        }

        SimpleTree(Node root)
        {
            this.root = root;
        }

        public string Name()
        {
            return "Simple";
        }

        public int[] GetArray()
        {
            List<int> list = new List<int>();

            Stack<(Node, bool)> stack = new Stack<(Node, bool)>();
            stack.Push((root, true));
            while (stack.Count > 0)
            {
                (Node node, bool left) = stack.Pop();
                if (node == null)
                    continue;
                if (left)
                {
                    stack.Push((node, false));
                    stack.Push((node.left, true));
                }
                else
                {
                    list.Add(node.x);
                    stack.Push((node.right, true));
                }
            }

            return list.ToArray();
        }

        public INodeTree Clone()
        {
            if (root == null)
                return new SimpleTree();

            Node newRoot = new Node(root.x);
            Stack<(Node, Node, bool)> stack = new Stack<(Node, Node, bool)>();
            stack.Push((root.left, newRoot, true));
            stack.Push((root.right, newRoot, false));
            while (stack.Count > 0)
            {
                (Node node, Node parent, bool left) = stack.Pop();
                if (node == null)
                    continue;
                if (left)
                    parent = parent.left = new Node(node.x);
                else
                    parent = parent.right = new Node(node.x);
                stack.Push((node.left, parent, true));
                stack.Push((node.right, parent, false));
            }

            return new SimpleTree(newRoot);
        }

        public void Insert(int x)
        {
            InsertNode(new Node(x));
        }

        public bool Find(int x)
        {
            return FindNode(x) != null;
        }

        public void Remove(int x)
        {
            RemoveNode(x);
        }

        void InsertNode(Node newNode)
        {
            if (newNode == null)
                return;
            ref Node child = ref FindNode(newNode.x);
            if (child == null)
                child = newNode;
        }

        ref Node FindNode(int x)
        {
            ref Node child = ref root;
            while (child != null && child.x != x)
                if (x < child.x)
                    child = ref child.left;
                else
                    child = ref child.right;
            return ref child;
        }

        void RemoveNode(int x)
        {
            ref Node child = ref FindNode(x);
            if (child != null)
            {
                Node left = child.left;
                Node right = child.right;
                child = null;
                InsertNode(left);
                InsertNode(right);
            }
        }
    }
}
