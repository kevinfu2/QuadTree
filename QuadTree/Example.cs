
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace QuadTreeLib
{
    class Example
    {
        static void Main(string[] args)
        {
            var tmp = File.ReadAllLines("a.txt");

            List<Tuple<double, double>> lists = new List<Tuple<double, double>>();
            foreach (var line in tmp)
            {
                var tmp2 = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                double x = double.Parse(tmp2[4]);
                double y = double.Parse(tmp2[5]);
                if (x < y)
                {
                    var tmpf = x;
                    x = y;
                    y = tmpf;
                }
                lists.Add(new Tuple<double, double>(x, y));
            }
            QuadTree X = new QuadTree(lists, 9000, 9000);
            var x2 = X.Nearst(1500, 500, 18000);
            Console.WriteLine(x2.Item1 + "::" + x2.Item2);
        }
    }
}
