using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace lesson._09.cs
{
    class SimpleNodeTree : INodeTree
    {
        SimpleNode root;

        public SimpleNode Root()
        {
            return root;
        }

        public void Insert(int x)
        {
            InsertNode(new SimpleNode(x));
        }

        public bool Find(int x)
        {
            return FindNode(x) != null;
        }

        public void Remove(int x)
        {
            RemoveNode(x);
        }

        void InsertNode(SimpleNode newNode)
        {
            if (newNode == null)
                return;
            ref SimpleNode child = ref FindNode(newNode.x);
            if(child == null)
                child = newNode;
        }

        ref SimpleNode FindNode(int x)
        {
            ref SimpleNode child = ref root;
            while (child != null && child.x != x)
                if (x < child.x)
                    child = ref child.left;
                else
                    child = ref child.right;
            return ref child;
        }

        public void RemoveNode(int x)
        {
            ref SimpleNode child = ref FindNode(x);
            if (child != null)
            {
                SimpleNode left = child.left;
                SimpleNode right = child.right;
                child = null;
                InsertNode(left);
                InsertNode(right);
            }
        }
    }
}
