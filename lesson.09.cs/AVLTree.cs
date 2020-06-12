﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace lesson._09.cs
{
    class AVLTree : INodeTree
    {
        class Node
        {
            public int x;
            public int levels;

            public Node left;
            public Node right;

            public Node(int x)
            {
                this.x = x;
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
            return new AVLTree(CloneNode(root));
        }

        // Base functions
        public void Insert(int x)
        {
            Node node = new Node(x);
            node.levels = LevelsFromNodeChilds(node);

            root = InsertNode(root, node);
        }
        public bool Find(int x)
        {
            return FindNode(root, x) != null;
        }
        public void Remove(int x)
        {
            root = RemoveNode(root, x);
        }

        Node SmallLeftRotate(Node node)
        {
            Node anotherNode = node.right;
            node.right = anotherNode.left;
            anotherNode.left = node;

            node.levels = LevelsFromNodeChilds(node);
            anotherNode.levels = LevelsFromNodeChilds(anotherNode);

            return anotherNode;
        }
        Node SmallRightRotate(Node node)
        {
            Node anotherNode = node.left;
            node.left = anotherNode.right;
            anotherNode.right = node;

            node.levels = LevelsFromNodeChilds(node);
            anotherNode.levels = LevelsFromNodeChilds(anotherNode);

            return anotherNode;
        }
        Node BigLeftRotate(Node node)
        {
            node.right = SmallRightRotate(node.right);
            return SmallLeftRotate(node);
        }
        Node BigRightRotate(Node node)
        {
            node.left = SmallLeftRotate(node.left);
            return SmallRightRotate(node);
        }

        // Additional functions

        // Utility functions
        Node InsertNode(Node parent, Node node)
        {
            if(parent== null)
                return node;

            if (node == null || node.x == parent.x)
                return parent;

            if (node.x < parent.x)
                parent.left = InsertNode(parent.left, node);
            else
                parent.right = InsertNode(parent.right, node);

            return RebalanceNode(parent);
        }

        Node RebalanceNode(Node node)
        {
            node.levels = LevelsFromNodeChilds(node);

            int balance = BalanceFromNode(node);

            if (balance== 2)
                if (BalanceFromNode(node.right) < 0)
                    return BigLeftRotate(node);
                else
                    return SmallLeftRotate(node);

            if (balance== -2)
                if (BalanceFromNode(node.left) > 0)
                    return BigRightRotate(node);
                else
                    return SmallRightRotate(node);

            return node;
        }

        int BalanceFromNode(Node node)
        {
            if (node == null) return 0;

            int balance = 0;
            if (node.right != null) balance += node.right.levels;
            if (node.left != null) balance -= node.left.levels;
            return balance;
        }

        int LevelsFromNodeChilds(Node node)
        {
            int levels = 0;
            if (node.left != null && levels < node.left.levels)
                levels = node.left.levels;
            if (node.right != null && levels < node.right.levels)
                levels = node.right.levels;
            return levels + 1;
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

        Node RemoveNode(Node node, int x)
        {
            if (node == null)
                return null;
            if (x < node.x)
                node.left = RemoveNode(node.left, x);
            else if (x > node.x)
                node.right = RemoveNode(node.right, x);
            else
            {
                if (node.right == null)
                    return node.left;
                Node minNode = FindMinNode(node.right);
                minNode.right = RemoveMinNode(node.right);
                minNode.left = node.left;
                return RebalanceNode(minNode);
            }
            return RebalanceNode(node);
        }

        Node FindMinNode(Node parent)
        {
            if (parent.left == null)
                return parent;
            else
                return FindMinNode(parent.left);
        }

        Node RemoveMinNode(Node parent)
        {
            if (parent.left == null)
                return parent.right;
            parent.left = RemoveMinNode(parent.left);
            return RebalanceNode(parent);
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
            newNode.right= CloneNode(node.right);
            newNode.levels = LevelsFromNodeChilds(newNode);

            return newNode;
        }
    }
}
