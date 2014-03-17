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
        private double kristofidesTime;
        private double bruteforceTime;
        private string kristofidesResult;
        private string bruteforceResult;

        public Research()
        {
            timeCreate = DateTime.Now;
            timeUpdate = DateTime.Now;
            generateGraph(5);
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

        public void generateGraph(int vertexCount)
        {
            graph = new GraphStructure.Graph(vertexCount);
            graph.generateGraph();
        }
    }
}
