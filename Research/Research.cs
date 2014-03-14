using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kristofides.Research
{
    class Research
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
        }
    }
}
