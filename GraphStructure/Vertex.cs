using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Kristofides.GraphStructure
{
    [XmlType("Vertex")]
    public class Vertex
    {
        [XmlElement]
        public int _id;
        [XmlElement]
        public double _x;
        [XmlElement]
        public double _y;
        //[XmlArrayItem("Edge")]
        //public List<Edge> _edgeList;

        public Vertex()
        {
        }

        public Vertex(int id,double x, double y)
        {
            _id = id;
            _x = x;
            _y = y;
            //_edgeList = new List<Edge>();
        }

        public Vertex(Vertex vertex)
        {
            _id = vertex._id;
            _x = vertex._x;
            _y = vertex._y;
            /*_edgeList = new List<Edge>();
            foreach (Edge edge in vertex._edgeList)
            {
                _edgeList.Add(new Edge(edge));
            }*/
        }

        /*public void AddEdge(Vertex secoundVertex)
        {
            //_edgeList.Add(new Edge(this, secoundVertex));
        }*/

        /*public double GetLengthToVertexById(int id)
        {
            double result=0;
            for (int i = 0; i < _edgeList.Count; i++)
            {
                if (_edgeList[i]._b._id == id)
                    result = _edgeList[i]._length;
            }

            return result;
        }*/
    }
}
