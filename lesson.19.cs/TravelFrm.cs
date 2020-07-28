using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace lesson._19.cs
{
    class TravelFrm : Form
    {
        double _point2pixelX;
        double _point2pixelY;
        int _labelWidth;
        int _labelHeight;

        Label _coordLabelCtrl;
        Label _countLabelCtrl;
        Label _timeLabelCtrl;
        Label _distanceLabelCtrl;
        Button _fullConnectionsBtnCtrl;
        Button _bruteforceBtnCtrl;
        Button _shortEdgeBtnCtrl;
        Button _branchAndBoundBtnCtrl;

        GraphCtrl _graphCtrl;

        public TravelFrm()
        {
            Console.WriteLine("TravelFrm constructed");
        }

        private void graphCtrl_MouseMove(object sender, MouseEventArgs e)
        {
            (double x, double y) = ((double)e.X / ClientSize.Width, (double)e.Y / ClientSize.Height);
            _coordLabelCtrl.Text = $"Coord: ({x:g3}, {y:g3})";
        }

        private void graphCtrl_MouseUp(object sender, MouseEventArgs e)
        {
            _countLabelCtrl.Text = $"Nodes: {_graphCtrl.Nodes.Count}";
        }

        private void fullConnectionsBtnCtrl_Click(object sender, EventArgs e)
        {
            GraphNode[] nodes = _graphCtrl.Nodes.ToArray();
            List<GraphEdge> edges = new List<GraphEdge>();
            for (int from = 0; from < nodes.Length - 1; ++from)
                for (int to = from + 1; to < nodes.Length; ++to)
                    edges.Add(new GraphEdge(from, to, GraphNode.Distance(nodes[from], nodes[to])));
            _graphCtrl.Edges = edges;
        }

        private void bruteforceBtnCtrl_Click(object sender, EventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            _graphCtrl.Edges = (new BruteforceTravel(_graphCtrl.Nodes)).Edges;
            sw.Stop();
            _timeLabelCtrl.Text = $"Time: {sw.Elapsed.TotalSeconds:g3}";
            double distance = 0;
            foreach (GraphEdge edge in _graphCtrl.Edges)
                distance += edge.distance;
            _distanceLabelCtrl.Text = $"Dist: {distance:g3}";
        }

        private void shortEdgeBtnCtrl_Click(object sender, EventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            _graphCtrl.Edges = (new ShortEdgeTravel(_graphCtrl.Nodes)).Edges;
            sw.Stop();
            _timeLabelCtrl.Text = $"Time: {sw.Elapsed.TotalSeconds:g3}";
            double distance = 0;
            foreach (GraphEdge edge in _graphCtrl.Edges)
                distance += edge.distance;
            _distanceLabelCtrl.Text = $"Dist: {distance:g3}";
        }

        private void branchAndBoundBtnCtrl_Click(object sender, EventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            _graphCtrl.Edges = (new BranchAndBoundTravel(_graphCtrl.Nodes)).Edges;
            sw.Stop();
            _timeLabelCtrl.Text = $"Time: {sw.Elapsed.TotalSeconds:g3}";
            double distance = 0;
            foreach (GraphEdge edge in _graphCtrl.Edges)
                distance += edge.distance;
            _distanceLabelCtrl.Text = $"Dist: {distance:g3}";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Graphics g = CreateGraphics();

            _point2pixelX = g.DpiX / 72.0f;
            _point2pixelY = g.DpiY / 72.0f;

            _labelWidth = (int)(150 * _point2pixelX);
            _labelHeight = (int)(20 * _point2pixelY);

            g.Dispose();

            _graphCtrl = new GraphCtrl();
            _coordLabelCtrl = new Label();
            _countLabelCtrl = new Label();
            _timeLabelCtrl = new Label();
            _distanceLabelCtrl = new Label();
            _fullConnectionsBtnCtrl = new Button();
            _bruteforceBtnCtrl = new Button();
            _shortEdgeBtnCtrl = new Button();
            _branchAndBoundBtnCtrl = new Button();

            _graphCtrl.BorderStyle = BorderStyle.Fixed3D;

            _coordLabelCtrl.TextAlign = ContentAlignment.MiddleLeft;
            _coordLabelCtrl.Text = "Coord: (x, y)";
            _coordLabelCtrl.BorderStyle = BorderStyle.Fixed3D;

            _countLabelCtrl.TextAlign = ContentAlignment.MiddleLeft;
            _countLabelCtrl.Text = "Nodes: N";
            _countLabelCtrl.BorderStyle = BorderStyle.Fixed3D;

            _timeLabelCtrl.TextAlign = ContentAlignment.MiddleLeft;
            _timeLabelCtrl.Text = "Time: sec";
            _timeLabelCtrl.BorderStyle = BorderStyle.Fixed3D;

            _distanceLabelCtrl.TextAlign = ContentAlignment.MiddleLeft;
            _distanceLabelCtrl.Text = "Dist: unit";
            _distanceLabelCtrl.BorderStyle = BorderStyle.Fixed3D;

            _fullConnectionsBtnCtrl.TextAlign = ContentAlignment.MiddleCenter;
            _fullConnectionsBtnCtrl.Text = "Full Connections";

            _bruteforceBtnCtrl.TextAlign = ContentAlignment.MiddleCenter;
            _bruteforceBtnCtrl.Text = "Bruteforce";

            _shortEdgeBtnCtrl.TextAlign = ContentAlignment.MiddleCenter;
            _shortEdgeBtnCtrl.Text = "Short Edge";

            _branchAndBoundBtnCtrl.TextAlign = ContentAlignment.MiddleCenter;
            _branchAndBoundBtnCtrl.Text = "Branch and Bound";

            Controls.Add(_graphCtrl);
            Controls.Add(_coordLabelCtrl);
            Controls.Add(_countLabelCtrl);
            Controls.Add(_timeLabelCtrl);
            Controls.Add(_distanceLabelCtrl);
            Controls.Add(_fullConnectionsBtnCtrl);
            Controls.Add(_bruteforceBtnCtrl);
            Controls.Add(_shortEdgeBtnCtrl);
            Controls.Add(_branchAndBoundBtnCtrl);

            _graphCtrl.MouseMove += new MouseEventHandler(graphCtrl_MouseMove);
            _graphCtrl.MouseUp += new MouseEventHandler(graphCtrl_MouseUp);

            _fullConnectionsBtnCtrl.Click += new EventHandler(fullConnectionsBtnCtrl_Click);
            _bruteforceBtnCtrl.Click += new EventHandler(bruteforceBtnCtrl_Click);
            _shortEdgeBtnCtrl.Click += new EventHandler(shortEdgeBtnCtrl_Click);
            _branchAndBoundBtnCtrl.Click += new EventHandler(branchAndBoundBtnCtrl_Click);

            MinimumSize = new Size(_labelWidth * 3, _labelWidth * 2);
            ClientSize = new Size(_labelWidth * 3, _labelWidth * 2);

            Console.WriteLine("TravelFrm loaded");
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            int minSize = Math.Min(ClientSize.Width - _labelWidth, ClientSize.Height);
            int labelSize = ClientSize.Width - minSize;

            _graphCtrl.Location = new Point(5, 5);
            _graphCtrl.Size = new Size(minSize - 10, minSize - 10);

            _coordLabelCtrl.Location = new Point(minSize + 5, 5);
            _coordLabelCtrl.Size = new Size(labelSize - 10, _labelHeight);

            _countLabelCtrl.Location = new Point(minSize + 5, _labelHeight + 5);
            _countLabelCtrl.Size = new Size(labelSize - 5, _labelHeight);

            _timeLabelCtrl.Location = new Point(minSize + 5, _labelHeight * 2 + 5);
            _timeLabelCtrl.Size = new Size(labelSize - 5, _labelHeight);

            _distanceLabelCtrl.Location = new Point(minSize + 5, _labelHeight * 3 + 5);
            _distanceLabelCtrl.Size = new Size(labelSize - 5, _labelHeight);

            _fullConnectionsBtnCtrl.Location = new Point(minSize + 5, _labelHeight * 4 + 5);
            _fullConnectionsBtnCtrl.Size = new Size(labelSize - 10, _labelHeight);

            _bruteforceBtnCtrl.Location = new Point(minSize + 5, _labelHeight * 5 + 5);
            _bruteforceBtnCtrl.Size = new Size(labelSize - 10, _labelHeight);

            _shortEdgeBtnCtrl.Location = new Point(minSize + 5, _labelHeight * 6 + 5);
            _shortEdgeBtnCtrl.Size = new Size(labelSize - 10, _labelHeight);

            _branchAndBoundBtnCtrl.Location = new Point(minSize + 5, _labelHeight * 7 + 5);
            _branchAndBoundBtnCtrl.Size = new Size(labelSize - 10, _labelHeight);
        }

    }
}
