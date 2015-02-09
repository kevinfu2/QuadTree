using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuadTreeLib
{
    public class QuadTree
    {
        public QuadTreeNode root;
        public QuadTree(List<Tuple<double, double>> data, double width, double height)
        {

            // Squarify the bounds.
            if (width > height) height = width;
            else width = height;

            root = new QuadTreeNode();
            foreach (var point in data)
            {
                root.Add(point, 0, 0, width, height);
            }
        }
        public Tuple<double, double> Nearst(double x, double y, double distance)
        {
            
            return this.root.Nearest(x, y, ref distance,null);
        }
    }
}
