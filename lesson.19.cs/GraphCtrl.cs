using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace lesson._19.cs
{
    class GraphNode
    {
        public double x;
        public double y;

        public GraphNode(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        static public double Distance(GraphNode a, GraphNode b)
        {
            double dx = a.x - b.x;
            double dy = a.y - b.y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    };

    class GraphEdge
    {
        public int from;
        public int to;

        public double distance;

        public GraphEdge(int from, int to, double distance)
        {
            this.from = from;
            this.to = to;
            this.distance = distance;
        }
    }

    class EdgeComparer : IComparer<GraphEdge>
    {
        int IComparer<GraphEdge>.Compare(GraphEdge a, GraphEdge b)
        {
            return a.distance.CompareTo(b.distance);
        }
    }

    class GraphCtrl : UserControl
    {
        private List<GraphNode> nodes = new List<GraphNode>();
        private List<GraphEdge> edges = null;
        private GraphNode[] nodeArray = null;

        public List<GraphNode> Nodes { get { return nodes; } }
        public List<GraphEdge> Edges { get { return edges; } set { edges = value; nodeArray = nodes.ToArray(); Invalidate(); } }

        Font font;

        public GraphCtrl()
        {
            font = new Font(FontFamily.GenericSansSerif, 10.0f);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (edges != null)
                foreach (GraphEdge edge in edges)
                {
                    (int X1, int Y1) = ((int)(nodeArray[edge.from].x * ClientSize.Width), (int)(nodeArray[edge.from].y * ClientSize.Height));
                    (int X2, int Y2) = ((int)(nodeArray[edge.to].x * ClientSize.Width), (int)(nodeArray[edge.to].y * ClientSize.Height));
                    e.Graphics.DrawLine(Pens.Red, X1, Y1, X2, Y2);
                }
            foreach (GraphNode node in nodes)
            {
                (int X, int Y) = ((int)(node.x * ClientSize.Width), (int)(node.y * ClientSize.Height));
                e.Graphics.FillRectangle(Brushes.Black, X - 5, Y - 5, 10, 10);
            }
            int cnt = 0;
            foreach (GraphNode node in nodes)
            {
                (int X, int Y) = ((int)(node.x * ClientSize.Width), (int)(node.y * ClientSize.Height));
                e.Graphics.DrawString($"{cnt}", font, Brushes.Blue, X + 5, Y + 5);
                ++cnt;
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            int removed = nodes.RemoveAll(
                (GraphNode node) =>
                {
                    (int X, int Y) = ((int)(node.x * ClientSize.Width), (int)(node.y * ClientSize.Height));
                    return Math.Abs(X - e.X) < 5 && Math.Abs(Y - e.Y) < 5;
                }
            );
            if (removed == 0)
            {
                nodes.Add(new GraphNode((double)e.X / ClientSize.Width, (double)e.Y / ClientSize.Height));
            }
            edges = null;
            nodeArray = null;
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
    }
}
