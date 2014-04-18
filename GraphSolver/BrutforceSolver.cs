using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kristofides.GraphStructure;
using System.Windows;
namespace Kristofides.GraphSolver
{   
    class Brutforce
    {
        private Graph original;
        private List<Vertex> result;

        public Brutforce(Graph graph)
        {
            this.original = graph;
        }

        public List<Vertex> Solve()
        {
            result = new List<Vertex>();

            List<Vertex> start = new List<Vertex>();
            start.Add(original.getVertexById(0));
            List<List<Vertex>> allPaths= new List<List<Vertex>>();
            allPaths = Recurse(start);
            allPaths = FilterByVertexCount(allPaths);
            allPaths = FilterByDestination(allPaths);
            double[] pathsLength = new double[allPaths.Count];

            for (int i = 0; i < allPaths.Count; i++)
            {
                pathsLength[i] = 0;
                for (int j = 0; j < allPaths[i].Count-1; j++)
                {
                    Vector vector= new Vector(allPaths[i][j]._x-allPaths[i][j+1]._x,allPaths[i][j]._y-allPaths[i][j+1]._y);
                    pathsLength[i] += vector.Length;
                }
            }

            int shortestPathIndex = 0;

            for (int i = 1; i < pathsLength.Length; i++)
            {
                if (pathsLength[i] < pathsLength[shortestPathIndex])
                {
                    shortestPathIndex = i;
                }
            }

            if (allPaths.Count == 0)
            {
                result = null;
            }
            else
            {
                result = allPaths[shortestPathIndex];
            }

            return result;
        }

        private List<List<Vertex>> FilterByVertexCount(List<List<Vertex>> inputPaths)
        {
            List<List<Vertex>> result = new List<List<Vertex>>();

            foreach (List<Vertex> path in inputPaths)
            {
                if (path.Count == original.getVertex().Count)
                {
                    result.Add(path);
                }
            }

            return result;
        }

        private List<List<Vertex>> FilterByDestination(List<List<Vertex>> inputPaths)
        {
            List<List<Vertex>> result = new List<List<Vertex>>();

            foreach (List<Vertex> path in inputPaths)
            {
                if (path.Last()._id == original.getVertex().Last()._id)
                {
                    result.Add(path);
                }
            }

            return result;
        }

        private List<List<Vertex>> Recurse(List<Vertex> inputPath)
        {
            List<List<Vertex>> result = new List<List<Vertex>>();
            bool isLast = true;
            List<Edge> tempEdges = original.getEdges(inputPath.Last());
            for (int i = 0; i < tempEdges.Count; i++)
            {
                if (CheckAccess(inputPath, tempEdges[i]._b))
                {
                    List<Vertex> tempPath = new List<Vertex>();

                    for(int j=0;j<inputPath.Count;j++)
                    {
                        tempPath.Add(inputPath[j]);
                    }

                    tempPath.Add(tempEdges[i]._b);
                    result.AddRange(Recurse(tempPath));
                    isLast = false;
                }
            }

            if (isLast)
            {
                result.Add(inputPath);
            }

            return result;
        }

        public bool CheckAccess(List<Vertex> denided, Vertex vertex)
        {
            foreach (Vertex denidedVertex in denided)
            {
                if (denidedVertex._id == vertex._id)
                    return false;
            }

            return true;
        }

        public List<Vertex> getResult()
        {
            return result;
        }

    }
}
