using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kristofides.GraphStructure;
using Kristofides.GraphSolver;

namespace Kristofides.WidthSearchOptimization
{
    class WidthSearchOptimizator
    {
        private SortedList<double,Item> itemsList;
        private Item lastItemsList;
        private List<String> pinaltyList;

        public WidthSearchOptimizator()
        {
            pinaltyList = new List<string>();
            pinaltyList.Add("positive1");
            pinaltyList.Add("positive2");
            pinaltyList.Add("positive3");
            pinaltyList.Add("negative1");
            pinaltyList.Add("negative2");
            pinaltyList.Add("negative3");
            pinaltyList.Add("combine1");
            pinaltyList.Add("combine2");
            pinaltyList.Add("combine3");
        }

        private void Pinalty(KristofidesSolver current, string method, double delta)
        {
            KristofidesSolver solver = new KristofidesSolver(current);
            switch (method)
            {
                case "positive1":
                    solver.PositivePenalty(1);
                    break;
                case "positive2":
                    solver.PositivePenalty(5);
                    break;
                case "positive3":
                    solver.PositivePenalty(10);
                    break;
                case "negative1":
                    solver.NegativePenalty(1);
                    break;
                case "negative2":
                    solver.NegativePenalty(5);
                    break;
                case "negative3":
                    solver.NegativePenalty(10);
                    break;
                case "combine1":
                    solver.CombinePenalty(1);
                    break;
                case "combine2":
                    solver.CombinePenalty(5);
                    break;
                case "combine3":
                    solver.CombinePenalty(10);
                    break;
            }

            Graph currentGraph = new Graph(solver.Solve());
            Item currentItem = new Item(currentGraph, method);
            itemsList.Add(currentItem.error + delta, currentItem);
        }

        private void Step(KristofidesSolver current)
        {
            itemsList = new SortedList<double, Item>();

            double delta = 0;
            foreach (var method in pinaltyList)
            {
                Pinalty(current, method, delta);
                delta += 0.0001;
            }

            lastItemsList=itemsList.Values.First();
        }

        private string CollectReport()
        {
            return lastItemsList.error.ToString() + " " + lastItemsList.length.ToString() + " " + lastItemsList.pinaltyMethod;

        }

        public string Iteration(KristofidesSolver current)
        {
            string report="";

            Step(current);
            report = CollectReport();

            return report;
        }

        public Graph GetLastBest()
        {
            return lastItemsList.modified;
        }
        
    }
}
