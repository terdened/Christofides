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
            pinaltyList.Add("positive5");
            pinaltyList.Add("positive10");
            pinaltyList.Add("negative1");
            pinaltyList.Add("negative5");
            pinaltyList.Add("negative10");
            pinaltyList.Add("combine1");
            pinaltyList.Add("combine5");
            pinaltyList.Add("combine10");
        }

        private void Pinalty(KristofidesSolver current, string method, double delta)
        {
            switch (method)
            {
                case "positive1":
                    current.PositivePenalty(1);
                    break;
                case "positive5":
                    current.PositivePenalty(5);
                    break;
                case "positive10":
                    current.PositivePenalty(10);
                    break;
                case "negative1":
                    current.NegativePenalty(1);
                    break;
                case "negative5":
                    current.NegativePenalty(5);
                    break;
                case "negative10":
                    current.NegativePenalty(10);
                    break;
                case "combine1":
                    current.CombinePenalty(1);
                    break;
                case "combine5":
                    current.CombinePenalty(5);
                    break;
                case "combine10":
                    current.CombinePenalty(10);
                    break;
            }

            Graph currentGraph = new Graph(current.Solve());
            Item currentItem = new Item(currentGraph, method);
            double Length = currentItem.length/10000;
            Length += delta / 100000;
            itemsList.Add(currentItem.error + Length, currentItem);
        }

        private void Pinalty(KristofidesSolver current, string method)
        {
            KristofidesSolver solver = current;
            switch (method)
            {
                case "positive1":
                    solver.PositivePenalty(1);
                    break;
                case "positive5":
                    solver.PositivePenalty(5);
                    break;
                case "positive10":
                    solver.PositivePenalty(10);
                    break;
                case "negative1":
                    solver.NegativePenalty(1);
                    break;
                case "negative5":
                    solver.NegativePenalty(5);
                    break;
                case "negative10":
                    solver.NegativePenalty(10);
                    break;
                case "combine1":
                    solver.CombinePenalty(1);
                    break;
                case "combine5":
                    solver.CombinePenalty(5);
                    break;
                case "combine10":
                    solver.CombinePenalty(10);
                    break;
            }
        }

        private void Step(KristofidesSolver current)
        {
            itemsList = new SortedList<double, Item>();

            List<double> delta = new List<double>();
            foreach (var method in pinaltyList)
            {
                Random randomer = new Random();
                double randomDelta= randomer.NextDouble();
                while (delta.Contains(randomDelta))
                {
                    randomDelta = randomer.NextDouble();
                }

                delta.Add(randomDelta);
            }

            int i = 0;
            foreach (var method in pinaltyList)
            {
                Pinalty(current, method, delta[i]);
                i++;
            }

            lastItemsList=itemsList.First().Value;
        }

        private string CollectReport()
        {
            return lastItemsList.pinaltyMethod + " " + lastItemsList.error.ToString() + " " + lastItemsList.length.ToString();
        }

        public string Iteration(KristofidesSolver current)
        {
            string report="";

            KristofidesSolver newSolver = new KristofidesSolver(current);
            Step(newSolver);
            Pinalty(current, lastItemsList.pinaltyMethod);

            report = CollectReport();

            return report;
        }

        ~WidthSearchOptimizator()
        {
            itemsList.Clear();
            pinaltyList.Clear();
        }
        
    }
}
