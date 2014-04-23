using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Kristofides.GraphStructure
{
    [XmlType("Edge")]
    public class Edge
    {
        [XmlIgnore]
        public Vertex _a;
        [XmlIgnore]
        public Vertex _b;
        [XmlElement]
        public int _aId;
        [XmlElement]
        public int _bId;
        [XmlElement]
        public double _length;

        public Edge()
        {
        }

        public Edge(Vertex a, Vertex b)
        {
            _a = a;
            _b = b;
            _aId = _a._id;
            _bId = _b._id;
            Vector tempVector = new Vector(_a._x - _b._x, _a._y-b._y);
            _length = tempVector.Length;
        }

        public Edge(Edge edge)
        {
            _a = edge._a;
            _b = edge._b;
            _aId = _a._id;
            _bId = _b._id;
            _length = edge._length;
        }

        public void updateVertex(List<Vertex> vertexes)
        {
            foreach (var vertex in vertexes)
            {
                if (vertex._id == _aId)
                    _a = vertex;
                if (vertex._id == _bId)
                    _b = vertex;

            }
        }
    }
}
