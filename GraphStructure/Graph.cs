using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kristofides.GraphStructure
{
    public class Graph
    {
        private List<Vertex> _vertexList;
        private List<Edge> _edgeList;
        private int _vertexCount;
        private int _minEdge;
        private int _maxEdge;


        public Graph(int vertexCount)
        {
            _vertexCount = vertexCount;
            _vertexList = new List<Vertex>();
            _edgeList = new List<Edge>();
        }

        public double[][] getMatrix()
        {
            double[][] result = new double[_vertexList.Count][];

            for (int i = 0; i < _vertexList.Count; i++)
            {
                result[i]=new double[_vertexList.Count];
                for (int j = 0; j < _vertexList.Count; j++)
                {
                    result[i][j] = _vertexList[i].GetLengthToVertexById(_vertexList[j]._id);
                }
            }

            return result;
        }

        public Vertex getVertexById(int id)
        {
            Vertex result = null;

            for (int i = 0; i < _vertexCount; i++)
            {
                if (_vertexList[i]._id == id)
                    result = _vertexList[i];
            }

            return result;
        }

        public void generateGraph()
        {
            Random rand = new Random();
            for (int i = 0; i < _vertexCount; i++)
            {
                
                double x = rand.NextDouble() * 200;
                double y = rand.NextDouble() * 200;

                _vertexList.Add(new Vertex(i,x,y));
            }

            for (int i = 0; i < _vertexCount; i++)
            {
                int count = (int)(rand.NextDouble()*(_vertexCount-1))+1;

                while (_vertexList[i]._edgeList.Count < count)
                {
                    int secound = (int)(rand.NextDouble() * _vertexCount);

                    while (secound == i)
                    {
                        secound = (int)(rand.NextDouble() * _vertexCount);
                    }

                    _vertexList[i].AddEdge(getVertexById(secound));
                    _vertexList[secound].AddEdge(getVertexById(i));
                }
            }
        }
    }
}
