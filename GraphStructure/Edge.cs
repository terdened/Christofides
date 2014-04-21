using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Kristofides.GraphStructure
{
    public class Edge
    {
        public Vertex _a;
        public Vertex _b;
        public double _length;

        public Edge(Vertex a, Vertex b)
        {
            _a = a;
            _b = b;
            Vector tempVector = new Vector(_a._x - _b._x, _a._y-b._y);
            _length = tempVector.Length;
        }

        public Edge(Edge edge)
        {
            _a = edge._a;
            _b = edge._b;
            Vector tempVector = new Vector(_a._x - _b._x, _a._y - _b._y);
            _length = edge._length;
        }
    }
}
