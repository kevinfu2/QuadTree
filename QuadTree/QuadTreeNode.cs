using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuadTreeLib
{
    public class QuadTreeNode
    {
        public bool Leaf = true;
        public bool Visited = false;
        public bool Scanned = false;
        public QuadTreeNode[] Nodes = new QuadTreeNode[4];
        public Tuple<double, double> Point = null;
        public double X = double.NaN;
        public double Y = double.NaN;
        public double x1 = double.NaN;
        public double y1 = double.NaN;
        public double x2 = double.NaN;
        public double y2 = double.NaN;

        public void Add(Tuple<double, double> d, double x1_, double y1_, double x2_, double y2_)
        {
            x1 = x1_;
            x2 = x2_;
            y1 = y1_;
            y2 = y2_;
            Insert(d);
        }

        public  void Insert(Tuple<double, double> Point)
        {
            if (double.IsNaN(Point.Item1) || double.IsNaN(Point.Item2)) return;
            if (this.Leaf)
            {
                double nx = this.X,
                    ny = this.Y;
                if (!double.IsNaN(nx))
                {
                    if ((Math.Abs(nx - Point.Item1) + Math.Abs(ny - Point.Item2)) < .01)
                    {
                        InsertChild(Point);
                    }
                    else
                    {
                        var nPoint = this.Point;
                        this.X = this.Y = double.NaN;
                        this.Point = null;
                        InsertChild(nPoint);
                        InsertChild( Point);
                    }
                }
                else
                {
                    this.X = Point.Item1; this.Y = Point.Item2; this.Point = Point;
                }
            }
            else
            {
                InsertChild( Point);
            }
        }

        private void InsertChild(Tuple<double, double> Point)
        {
           double sx = (x1 + x2) * .5,
                sy = (y1 + y2) * .5;
            bool right = Point.Item1 >= sx,
               bottom = Point.Item2 >= sy;
            int hori = 0, vert = 0;
            if (right) hori = 1;
            if (bottom) vert = 1;
            int i = (hori << 1)+vert;

            this.Leaf = false;
            if (this.Nodes[i] == null)
            {
                this.Nodes[i] = new QuadTreeNode();
                this.Nodes[i].x1 = x1;
                this.Nodes[i].x2 = x2;
                this.Nodes[i].y1 = y1;
                this.Nodes[i].y2 = y2;
            }
            if (right) this.Nodes[i].x1 = sx; else this.Nodes[i].x2 = sx;
            if (bottom) this.Nodes[i].y1 = sy; else this.Nodes[i].y2 = sy;
            this.Nodes[i].Insert(Point);
        }



        internal Tuple<double, double> Nearest(double x, double y, ref double d, Tuple<double, double> best)
        {
            this.Visited = true;
            if (x < x1 - d || x > x2 + d || y < y1 - d || y > y2 + d)
            {
                return best;
            }
            var p = this.Point;
            if (p != null)
            {
                this.Scanned = true;
                double dx = p.Item1 - x, dy = p.Item2 - y, dis = Math.Sqrt(dx * dx + dy * dy);
                if (dis < d)
                {
                    d = dis;
                    best = p;
                }
            }
            var kids = this.Nodes;
            bool rl = (2 * x > x1 + x2), bt = (2 * y > y1 + y2);
            int right = rl ? 1 : 0, bottom = bt ? 1 : 0;
            if (kids[bottom * 2 + right] != null) best = kids[bottom * 2 + right].Nearest(x, y,  ref d, best);
            if (kids[bottom * 2 + (1 - right)] != null) best = kids[bottom * 2 + (1 - right)].Nearest(x, y, ref d, best);
            if (kids[(1 - bottom) * 2 + right] != null) best = kids[(1 - bottom) * 2 + right].Nearest(x, y,ref  d, best);
            if (kids[(1 - bottom) * 2 + (1 - right)] != null) best = kids[(1 - bottom) * 2 + (1 - right)].Nearest(x, y, ref d, best);

            return best;
        }
    }
}
