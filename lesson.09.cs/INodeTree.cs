using System;
using System.Collections.Generic;
using System.Text;

namespace lesson._09.cs
{
    interface INodeTree
    {
        public SimpleNode Root();

        public void Insert(int x);
        public bool Find(int x);
        public void Remove(int x);
    }
}
