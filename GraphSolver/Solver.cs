using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kristofides.GraphStructure;

namespace Kristofides.GraphSolver
{
    class Solver
    {
        private class Position {

            public List<Edge> edges;
            public int index1;
            public int index2;

            public Position(List<Edge> edges) 
            {
                this.edges = edges;
                index1 = 0;
                index2 = 1;
            }

            public bool next() {
                index2++;
                if (index2 == edges.Count) {
                    index1++;
                    index2 = index1 + 1;
                } else if (index2 > edges.Count) {
                    index1 = 0;
                    index2 = 1;
                    return true;
                }
                return false;
            }
        }

        private Graph original;
        private Graph graph;
        private List<Edge> denied = new List<Edge>();
        private List<Edge> originalDenied;
        private List<Position> position = new List<Position>();
        private bool unreal = false;

        public Solver(Graph graph, List<Edge> denied) {
            this.original = graph;
            this.originalDenied = denied;
            this.denied.AddRange(denied);

            prepare();
            if (unreal) return;
            foreach (Vertex v in this.graph.getVertex()) {
                List<Edge> lst = this.graph.getEdges(v);
                if (lst.Count > 2) {
                    position.Add(new Position(lst));
                }
            }
            deny();

        }

        public Graph getOstov(Graph original) {
            Graph graph = new Graph();
            graph.getVertex().AddRange(original.getVertex());

            Dictionary<Vertex, List<Vertex>> map = new Dictionary<Vertex, List<Vertex>>();

            foreach (Vertex v in original.getVertex()) 
            {
                List<Vertex> s = new List<Vertex>();
                s.Add(v);
                map.Add(v, s);
            }

            SortedList<double, Edge> edges = new SortedList<double, Edge>();

            foreach (Edge ed in original.getEdgeList())
            {
                edges.Add(ed._length, ed);
            }
            
            int i=edges.Count;
            while (i < original.getVertex().Count - 1) {
                i--;
                Edge e = edges.Values[i];

                List<Vertex> set = map[e._a];

                if (set == map[e._b]) continue;

                set.AddRange(map[e._b]);
                foreach (Vertex tv in map[e._b])
                    map.Add(tv, set);

                graph.getEdges().Add(e);
            }
            return graph;
        }

        private void prepare() 
        {
            graph = new Graph();
            graph.getVertex().AddRange(original.getVertex());

            Dictionary<Vertex, List<Vertex>> map = new Dictionary<Vertex, List<Vertex>>(original.getVertex().Count);
            
            foreach (Vertex v in original.getVertex()) {
                List<Vertex> s = new List<Vertex>();
                s.Add(v);
                map.Add(v, s);
            }


            SortedList<double, Edge> edges = new SortedList<double, Edge>();

            foreach(Edge ed in original.getEdgeList())
            {
                edges.Add(ed._length,ed);
            }
            
            int i=edges.Count;

            while (graph.getEdges().Count < original.getVertex().Count - 1) {
                i--;
                Edge e = edges.Values[i];
                if (e == null) {
                    unreal = true;
                    return;
                }
                if (denied.IndexOf(e)!=-1) continue;

                List<Vertex> set = map[e._a];

                if (set == map[e._b]) continue;

                set.AddRange(map[e._b]);

                foreach (Vertex tv in map[e._b])
                    map.Add(tv, set);

                graph.getEdges().Add(e);
            }

        }

        private void deny() {
            denied.Clear();
            denied.AddRange(originalDenied);

            foreach (Position p in position)
                for (int i = 0; i < p.edges.Count; i++)
                    if (i != p.index1 && i != p.index2) 
                    {
                         denied.Add(p.edges[i]);
                    }

        }

        private bool next() {
            foreach (Position p in position)
                if (!p.next()) {
                    prepare();
                    deny();
                    return true;
                }
            return false;
        }

        public Solver min(float min, int depth) {
            if (unreal) return this;
            if (position.Count<=0)
                return this;

            if (this.calc(false) >= min) return this;

            Solver res = new Solver(original, denied).min(min, depth + 1);
            float calc = res.calc(true);
            if (min >= calc)
                min = calc;
            else
                res = this;

            while (next()) {
                Solver ostov = new Solver(original, denied).min(min, depth + 1);
                calc = ostov.calc(true);
                if (min > calc) {
                    min = calc;
                    res = ostov;
                }
            }
            return res;
        }

        private float calc(bool check) {

            if (check) {
                if (unreal) return float.PositiveInfinity;
                if (position.Count>0) return float.PositiveInfinity;
            }

            float s = 0;
            foreach (Edge e in graph.getEdges()) {
                s += (float)e._length;
            }

            return s;
        }

        public Graph getGraph() {
            return graph;
        }

    }
}
