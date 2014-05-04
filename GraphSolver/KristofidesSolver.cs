using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kristofides.GraphStructure;

namespace Kristofides.GraphSolver
{
    class KristofidesSolver
    {
        private Graph original;
        private Graph modified;

        public KristofidesSolver(Graph graph)
        {
            original = new Graph(graph);
            modified = new Graph(original);
            FirstPenalty(100000);
        }

        public KristofidesSolver(KristofidesSolver solver)
        {
            original = new Graph(solver.original);
            modified = new Graph(solver.modified);
        }

        public void Reset()
        {
            modified = new Graph();
            foreach (Edge edge in original._edgeList)
            {
                modified.addEdge(edge);
            }
            FirstPenalty(1000);
        }

        public Graph GetModified()
        {
            return modified;
        }

        public void SetModified(Graph newGraph)
        {
            modified=new Graph(newGraph);
        }

        public Graph Solve()
        {
            Graph skeleton = GetSkeleton(modified);
            return skeleton;
        }

        public Graph GetSkeleton(Graph inputGraph)
        {
            Graph result= new Graph();
            //result.updateVertex();
            SortedList<double, Edge> edgeSortedList = new SortedList<double, Edge>();

            foreach (Edge edge in inputGraph.getEdges())
            {
                edgeSortedList.Add(edge._length, edge);
            }

            while (result.getEdges().Count < inputGraph.getVertex().Count-1)
            {
                int i=0;

                while (true)
                {
                    Edge newEdge = edgeSortedList.Values[i];
                    if (!CheckAllow(result,newEdge._a,newEdge._b,new List<Vertex>()))
                    {
                        result.addEdge(newEdge);
                        edgeSortedList.Remove(newEdge._length);
                        break;
                    }
                    i++;
                }

            }

            return result;
        }

        public void PositivePenalty(double value)
        {
            Graph skeleton = GetSkeleton(modified);
            for (int i = 0; i < modified.getVertex().Count; i++)
            {
                if (skeleton.getEdges(skeleton.getVertex()[i]).Count > 2)
                {
                    int vertexId = skeleton.getVertex()[i]._id;

                    for (int j = 0; j < modified.getEdgeList().Count; j++)
                    {
                        if ((modified._edgeList[j]._a._id == vertexId) || (modified._edgeList[j]._b._id == vertexId))
                        {
                            modified._edgeList[j]._length += value * (skeleton.getEdges(skeleton.getVertex()[i]).Count - 2);
                        }
                    }
                }
            }
        }

        public void NegativePenalty(double value)
        {
            Graph skeleton = GetSkeleton(modified);
            for (int i = 0; i < modified.getVertex().Count; i++)
            {
                if (skeleton.getEdges(skeleton.getVertex()[i]).Count == 1)
                {
                    int vertexId = skeleton.getVertex()[i]._id;

                    for (int j = 0; j < modified.getEdgeList().Count; j++)
                    {
                        if ((modified._edgeList[j]._a._id == vertexId) || (modified._edgeList[j]._b._id == vertexId))
                        {
                            modified._edgeList[j]._length -= value;
                        }
                    }
                }
            }
        }

        public void CombinePenalty(double value)
        {
            Graph skeleton = GetSkeleton(modified);
            for (int i = 0; i < modified.getVertex().Count; i++)
            {
                if (skeleton.getEdges(skeleton.getVertex()[i]).Count == 1)
                {
                    int vertexId = skeleton.getVertex()[i]._id;

                    for (int j = 0; j < modified.getEdgeList().Count; j++)
                    {
                        if ((modified._edgeList[j]._a._id == vertexId) || (modified._edgeList[j]._b._id == vertexId))
                        {
                            modified._edgeList[j]._length -= value;
                        }
                    }
                }

                if (skeleton.getEdges(skeleton.getVertex()[i]).Count > 2)
                {
                    int vertexId = skeleton.getVertex()[i]._id;

                    for (int j = 0; j < modified.getEdgeList().Count; j++)
                    {
                        if ((modified._edgeList[j]._a._id == vertexId) || (modified._edgeList[j]._b._id == vertexId))
                        {
                            modified._edgeList[j]._length += value * (skeleton.getEdges(skeleton.getVertex()[i]).Count - 2);
                        }
                    }
                }
            }
        }

        private void FirstPenalty(double value)
        {
            int lastVertexId = original._vertexList.Count - 1;

            for (int j = 0; j < modified.getEdgeList().Count; j++)
            {
                if ((modified._edgeList[j]._a._id == 0) || (modified._edgeList[j]._b._id == 0))
                {
                    modified._edgeList[j]._length += value;
                }
            }

            for (int j = 0; j < modified.getEdgeList().Count; j++)
            {
                if ((modified._edgeList[j]._a._id == lastVertexId) || (modified._edgeList[j]._b._id == lastVertexId))
                {
                    modified._edgeList[j]._length += value;
                }
            } 
        }

        private bool CheckAllow(Graph inputGraph, Vertex a, Vertex b, List<Vertex> path)
        {
            bool result = false;
            
            foreach(Edge edge in inputGraph.getEdges())
            {

                if (edge._a._id == a._id)
                {
                    if(edge._b._id == b._id)
                    {
                        result = true;
                        break;
                    }
                    else
                    if (!path.Contains(edge._b))
                    {
                        List<Vertex> newPath=new List<Vertex>();

                        foreach(var vertex in path)
                        {
                            newPath.Add(vertex);
                        }

                        newPath.Add(a);
                        result=CheckAllow(inputGraph, edge._b, b, newPath);
                        if (result)
                            break;
                    }
                }
                else
                if (edge._b._id == a._id)
                {
                    if (edge._a._id == b._id)
                    {
                        result = true;
                        break;
                    }
                    else
                    if (!path.Contains(edge._a))
                    {
                        List<Vertex> newPath = new List<Vertex>();

                        foreach (var vertex in path)
                        {
                            newPath.Add(vertex);
                        }

                        newPath.Add(a);
                        result=CheckAllow(inputGraph, edge._a, b, newPath);
                        if (result)
                            break;
                    }
                }
            }

            return result;
        }
    }
}
