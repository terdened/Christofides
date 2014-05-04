using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kristofides.GraphStructure;

namespace Kristofides.WidthSearchOptimization
{
    struct Item
    {
        //public Graph modified;
        public string pinaltyMethod;
        public int error;
        public double length;


        public Item(GraphStructure.Graph graph, string pinalty)
        {
            //modified = new Graph(graph);

            pinaltyMethod = pinalty;
            error = 0;
            length = 0;
            estimateError(graph);
            estimateLength(graph);
        }

        private void estimateError(GraphStructure.Graph graph)
        {
            GraphSolver.KristofidesSolver solver = new GraphSolver.KristofidesSolver(graph);
            Graph skeleton = solver.Solve();
            error = 0;
            foreach (Vertex vertex in skeleton._vertexList)
            {
                error += Math.Abs(skeleton.getEdges(vertex).Count - 2);
            }
        }

        private void estimateLength(GraphStructure.Graph graph)
        {
            GraphSolver.KristofidesSolver solver = new GraphSolver.KristofidesSolver(graph);
            Graph skeleton = solver.Solve();
            length = 0;
            foreach (Edge edge in skeleton._edgeList)
            {
                length += edge._length;
            }
        }
    }
}
