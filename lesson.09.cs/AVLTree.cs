using System;
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
            node.levels = LevelsFromChilds(node);

            root = InsertNode(null, root, Node.ParentPos.root, node);

            //PropagateLevelsFrom(node);
            //Rebalance();
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

            //root = DetachNode(root, node);
            //root = InsertNode(root, node.left);
            //root = InsertNode(root, node.right);

            //Rebalance();
        }

        Node SmallLeftRotate(Node node)
        {
            Node anotherNode = node.right;
            node.right = anotherNode.left;
            anotherNode.left = node;

            anotherNode.parent = node.parent;
            anotherNode.parentPos = node.parentPos;

            node.levels = LevelsFromChilds(node);
            anotherNode.levels = LevelsFromChilds(anotherNode);

            if (node.right != null)
            {
                node.right.parent = node;
                node.right.parentPos = Node.ParentPos.right;
            }

            node.parent = anotherNode;
            node.parentPos = Node.ParentPos.left;

            return anotherNode;
        }
        Node SmallRightRotate(Node node)
        {
            Node anotherNode = node.left;
            node.left = anotherNode.right;
            anotherNode.right = node;

            anotherNode.parent = node.parent;
            anotherNode.parentPos = node.parentPos;

            node.levels = LevelsFromChilds(node);
            anotherNode.levels = LevelsFromChilds(anotherNode);

            if (node.left != null)
            {
                node.left.parent = node;
                node.left.parentPos = Node.ParentPos.left;
            }

            node.parent = anotherNode;
            node.parentPos = Node.ParentPos.right;

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
        //void Rebalance()
        //{
        //    root = RebalanceNode(root);
        //}

        // Utility functions
        Node InsertNode(Node parent, Node link, Node.ParentPos parentPos, Node node)
        {
            if(link == null)
            {
                node.parent = parent;
                node.parentPos = parentPos;
                return node;
            }

            if (node.x == link.x)
                return link;

            if (node.x < link.x)
                link.left = InsertNode(link, link.left, Node.ParentPos.left, node);
            else
                link.right = InsertNode(link, link.right, Node.ParentPos.right, node);

            link.levels = LevelsFromChilds(link);

            return RebalanceNode(link);
        }

        Node RebalanceNode(Node node)
        {
            int balanceLink = NodeBalance(node);

            if (balanceLink == 2)
                if (NodeBalance(node.right) < 0)
                    return BigLeftRotate(node);
                else
                    return SmallLeftRotate(node);

            if (balanceLink == -2)
                if (NodeBalance(node.left) > 0)
                    return BigRightRotate(node);
                else
                    return SmallRightRotate(node);

            return node;
        }

        //Node InsertNode(Node parent, Node node, Node.ParentPos parentPos, Node newNode)
        //{
        //    if (newNode == null)
        //        return node;

        //    if (node == null)
        //    {
        //        newNode.parent = parent;
        //        newNode.parentPos = parentPos;
        //        return newNode;
        //    }

        //    if (node.x < parent.x)
        //        parent.left = InsertNode(parent, parent.left, Node.ParentPos.left, node);
        //    else
        //        parent.right = InsertNode(parent, parent.right, Node.ParentPos.right, node);

        //    return node;
        //}

        //Node InsertNode(Node parent, Node node)
        //{
        //    if (node == null)
        //        return parent;

        //    if (parent == null)
        //    {
        //        node.parent = null;
        //        node.parentPos = Node.ParentPos.root;
        //        return node;
        //    }

        //    if (node.x < parent.x) {
        //        if (parent.left == null)
        //        {
        //            parent.left = node;
        //            node.parent = parent;
        //            node.parentPos = Node.ParentPos.left;
        //        }
        //        else
        //            InsertNode(parent.left, node);
        //    }
        //    else
        //    {
        //        if (parent.right == null)
        //        {
        //            parent.right = node;
        //            node.parent = parent;
        //            node.parentPos = Node.ParentPos.right;
        //        }
        //        else
        //            InsertNode(parent.right, node);
        //    }

        //    return parent;
        //}

        int NodeBalance(Node node)
        {
            if (node == null) return 0;

            int balance = 0;
            if (node.right != null) balance += node.right.levels;
            if (node.left != null) balance -= node.left.levels;
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

        //void PropagateLevelsFrom(Node node)
        //{
        //    if (node == null)
        //        return;
        //    node.levels = LevelsFromChilds(node);
        //    PropagateLevelsFrom(node.parent);
        //}

        Node FindNode(Node node, int x)
        {
            if (node == null || node.x == x)
                return node;
            if (x < node.x)
                return FindNode(node.left, x);
            else
                return FindNode(node.right, x);
        }

        //Node DetachNode(Node root, Node node)
        //{
        //    switch (node.parentPos)
        //    {
        //        case Node.ParentPos.root:
        //            return null;
        //        case Node.ParentPos.left:
        //            node.parent.left = null;
        //            break;
        //        case Node.ParentPos.right:
        //            node.parent.right = null;
        //            break;
        //    }
        //    PropagateLevelsFrom(node.parent);

        //    node.parent = null;
        //    node.parentPos = Node.ParentPos.root;

        //    return root;
        //}

        //Node RebalanceNode(Node node)
        //{
        //    if (node == null)
        //        return node;

        //    int levelsLeft = node.left == null ? 0 : node.left.levels;
        //    int levelsRight = node.right == null ? 0 : node.right.levels;

        //    return node;
        //}

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
