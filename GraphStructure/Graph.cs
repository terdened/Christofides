using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kristofides.GraphStructure
{
    class Graph
    {
        private LinkedList<Vertex> vertexList;
        private LinkedList<Edge> edgeList;
        private double[][] matrix;

        Graph(double[][] matrix)
        {
            this.matrix = matrix;
        }
    }
}
