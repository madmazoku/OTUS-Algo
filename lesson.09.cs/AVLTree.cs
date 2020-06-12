using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace lesson._09.cs
{
    class AVLTree : INodeTree
    {
        class Node
        {
            public enum ParentPos
            {
                root,
                left,
                right
            };

            public int x;
            public int levels;

            public Node parent;
            public ParentPos parentPos;

            public Node left;
            public Node right;

            public Node(int x)
            {
                this.x = x;
                this.parent = null;
                this.parentPos = ParentPos.root;
            }
        };

        Node root;

        public AVLTree() { root = null;  }

        AVLTree(Node root) { this.root = root; }

        public string Name()
        {
            return "AVL";
        }
        public int[] GetArray()
        {
            List<int> list = new List<int>();
            FillList(root, list);
            return list.ToArray();
        }
        public INodeTree Clone()
        {
            return new AVLTree(CloneNode(root, null, Node.ParentPos.root));
        }

        // Base functions
        public void Insert(int x)
        {
            Node node = new Node(x);
            root = InsertNode(root, node);
            PropagateLevelsFrom(node);
            Rebalance();
        }
        public bool Find(int x)
        {
            return FindNode(root, x) != null;
        }
        public void Remove(int x)
        {
            Node node = FindNode(root, x);
            if (node == null)
                return;

            root = DetachNode(root, node);
            root = InsertNode(root, node.left);
            root = InsertNode(root, node.right);

            Rebalance();
        }

        // Additional functions
        void Rebalance()
        {
            root = RebalanceNode(root);
        }

        // Utility functions
        Node InsertNode(Node parent, Node node)
        {
            if (node == null)
                return parent;

            if (parent == null)
            {
                node.parent = null;
                node.parentPos = Node.ParentPos.root;
                return node;
            }

            if (node.x < parent.x) {
                if (parent.left == null)
                {
                    parent.left = node;
                    node.parent = parent;
                    node.parentPos = Node.ParentPos.left;
                }
                else
                    InsertNode(parent.left, node);
            }
            else
            {
                if (parent.right == null)
                {
                    parent.right = node;
                    node.parent = parent;
                    node.parentPos = Node.ParentPos.right;
                }
                else
                    InsertNode(parent.right, node);
            }
            return parent;
        }

        int NodeBalance(Node node)
        {
            if (node == null) return 0;

            int balance = 0;
            if (node.left != null) balance += node.left.levels;
            if (node.right != null) balance -= node.right.levels;
            return balance;
        }

        int LevelsFromChilds(Node node)
        {
            int levels = 0;
            if (node.left != null && levels < node.left.levels)
                levels = node.left.levels;
            if (node.right != null && levels < node.right.levels)
                levels = node.right.levels;
            return levels + 1;
        }

        void PropagateLevelsFrom(Node node)
        {
            if (node == null)
                return;
            node.levels = LevelsFromChilds(node);
            PropagateLevelsFrom(node.parent);
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

        Node DetachNode(Node root, Node node)
        {
            switch (node.parentPos)
            {
                case Node.ParentPos.root:
                    return null;
                case Node.ParentPos.left:
                    node.parent.left = null;
                    break;
                case Node.ParentPos.right:
                    node.parent.right = null;
                    break;
            }
            PropagateLevelsFrom(node.parent);

            node.parent = null;
            node.parentPos = Node.ParentPos.root;

            return root;
        }

        Node RebalanceNode(Node node)
        {
            if (node == null)
                return node;

            int levelsLeft = node.left == null ? 0 : node.left.levels;
            int levelsRight = node.right == null ? 0 : node.right.levels;

            return node;
        }

        void FillList(Node node, List<int> list)
        {
            if (node == null)
                return;
            FillList(node.left, list);
            list.Add(node.x);
            FillList(node.right, list);
        }

        Node CloneNode(Node node, Node newNodeParent, Node.ParentPos newNodeParentPos)
        {
            if (node == null)
                return null;

            Node newNode = new Node(node.x);
            newNode.parent = newNodeParent;
            newNode.parentPos = newNodeParentPos;
            newNode.left = CloneNode(node.left, newNode, Node.ParentPos.left);
            newNode.right= CloneNode(node.right, newNode, Node.ParentPos.right);
            newNode.levels = LevelsFromChilds(newNode);

            return newNode;
        }
    }
}
