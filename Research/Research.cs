using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kristofides.Research
{
    public class Research
    {
        private DateTime timeCreate;
        private DateTime timeUpdate;
        private GraphStructure.Graph graph;
        public double kristofidesTime;
        public double bruteforceTime;
        public string kristofidesResult;
        public string bruteforceResult;

        public Research()
        {
            timeCreate = DateTime.Now;
            timeUpdate = DateTime.Now;
            kristofidesTime = 0;
            bruteforceTime = 0;
            kristofidesResult = "";
            bruteforceResult = "";
        }

        public DateTime getTimeCreate()
        {
            return timeCreate;
        }

        public DateTime getTimeUpdate()
        {
            return timeUpdate;
        }

        public GraphStructure.Graph getGraph()
        {
            return graph;
        }

        public void generateGraph(int vertexCount, int minEdge, int maxEdge)
        {
            graph = new GraphStructure.Graph(vertexCount, minEdge, maxEdge);
            graph.generateGraph();
            timeUpdate = DateTime.Now;
        }

        public void generateGraph(GraphStructure.Graph newGraph)
        {
            graph = newGraph;
            timeUpdate = DateTime.Now;
        }

        public void initGraph()
        {
            graph = new GraphStructure.Graph();
            timeUpdate = DateTime.Now;
        }
    }
}
