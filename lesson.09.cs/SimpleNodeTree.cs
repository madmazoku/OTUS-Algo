using System.Collections.Generic;

namespace lesson._09.cs
{
    class SimpleNodeTree : INodeTree
    {
        class Node
        {
            public int x;
            public Node left;
            public Node right;

            public Node(int x) { this.x = x; }
        };

        Node root;

        public SimpleNodeTree()
        {
            this.root = null;
        }

        SimpleNodeTree(Node root)
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
            FillList(root, list);
            return list.ToArray();
        }

        public INodeTree Clone()
        {
            return new SimpleNodeTree(CloneNode(root));
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

            Node newNode = new Node(node.x);
            newNode.left = CloneNode(node.left);
            newNode.right = CloneNode(node.right);

            return newNode;
        }
    }
}
