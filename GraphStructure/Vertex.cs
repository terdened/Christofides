using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kristofides.GraphStructure
{
    public class Vertex
    {
        public int _id;
        public double _x;
        public double _y;
        public List<Edge> _edgeList;

        public Vertex(int id,double x, double y)
        {
            _id = id;
            _x = x;
            _y = y;
            _edgeList = new List<Edge>();
        }

        public void AddEdge(Vertex secoundVertex)
        {
            _edgeList.Add(new Edge(this, secoundVertex));
        }

        public double GetLengthToVertexById(int id)
        {
            double result=0;
            for (int i = 0; i < _edgeList.Count; i++)
            {
                if (_edgeList[i]._b._id == id)
                    result = _edgeList[i]._length;
            }

            return result;
        }
    }
}
