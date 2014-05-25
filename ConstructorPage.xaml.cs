using Kristofides.GraphStructure;
using Kristofides.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kristofides
{
    /// <summary>
    /// Interaction logic for ConstructorPage.xaml
    /// </summary>
    public partial class ConstructorPage : Page
    {
        String state = "";
        Research.Research _research;
        int pointId = 0;
        Vertex selected=null;

        public ConstructorPage(Research.Research research)
        {
            InitializeComponent();
            RefreshCanvas();
            _research = research;
            if (_research != null)
            {
                if(_research.getGraph() != null)
                {
                    ShowGraph();
                    pointId = research.getGraph()._vertexList.Count;
                }
                else
                {
                    _research.initGraph();
                }
            }
            else
            {
                _research = new Research.Research();
            }
        }

        private void RefreshCanvas()
        {
            GraphCanvas.Children.Clear();
            Rectangle back = new Rectangle();
            back.Width = 700;
            back.Height = 500;
            back.Fill = System.Windows.Media.Brushes.White;
            back.Stroke = System.Windows.Media.Brushes.Gray;
            GraphCanvas.Children.Add(back);
        }

        private void CreatePointButton_Click(object sender, RoutedEventArgs e)
        {
            state = "CreatePoint";
            CreatePointButton.BorderBrush = System.Windows.Media.Brushes.Green;
            CreateLineButton.BorderBrush = System.Windows.Media.Brushes.Black;
            CreatePointButton.BorderThickness = new Thickness(5);
            CreateLineButton.BorderThickness = new Thickness(1);
        }

        private void CreateLineButton_Click(object sender, RoutedEventArgs e)
        {
            state = "CreateLine";
            CreateLineButton.BorderBrush = System.Windows.Media.Brushes.Green;
            CreatePointButton.BorderBrush = System.Windows.Media.Brushes.Black;
            CreateLineButton.BorderThickness = new Thickness(5);
            CreatePointButton.BorderThickness = new Thickness(1);
        }

        private bool IsSelect(Point p1, Point p2)
        {
            Vector vector= new Vector(p1.X-p2.X, p1.Y-p2.Y);

            if (vector.Length > 10)
                return false;
            else
                return true;
        }

        private Vertex SelectVertex(Point p)
        {
            Vertex result = null;

            foreach(var vertex in _research.getGraph()._vertexList)
            {
                if(IsSelect(p, new Point(vertex._x+44,vertex._y+4)))
                {
                    return vertex;
                }
            }

            return result;
        }

        private void Canvas_Click(object sender, MouseButtonEventArgs e)
        {
            if (state == "CreatePoint")
            {
                Point p = e.GetPosition(this);
                GraphStructure.Vertex vetex = new GraphStructure.Vertex();
                vetex._x = p.X-44;
                vetex._y = p.Y-4;
                vetex._id = pointId++;
                _research.getGraph().addVertex(vetex);
                ShowGraph();
            }
            else
            if (state == "CreateLine")
            {
                Point p = e.GetPosition(this);

                if(selected==null)
                {
                    selected = SelectVertex(p);
                }
                else
                {
                    if (SelectVertex(p)!=null)
                    {
                        Edge edge = new Edge(selected, SelectVertex(p));
                        edge._title = Microsoft.VisualBasic.Interaction.InputBox("Input edge title", "Input", "", 100, 100);

                        _research.getGraph().addEdge(edge);
                        selected = null;
                        ShowGraph();
                    }
                }
            }
        }

        public void ShowGraph()
        {
            RefreshCanvas();
            List<EdgeView> edgeViewList = new List<EdgeView>();
            List<Edge> edgeList = _research.getGraph().getEdgeList();

            for (int i = 0; i < edgeList.Count; i++)
            {
                edgeViewList.Add(new EdgeView(edgeList[i]._a._x, edgeList[i]._a._y, edgeList[i]._b._x, edgeList[i]._b._y, (int)edgeList[i]._length, edgeList[i]._title));
                this.GraphCanvas.Children.Add(edgeViewList.Last().line);
                this.GraphCanvas.Children.Add(edgeViewList.Last().text);
                if(edgeViewList.Last().title!=null)
                    this.GraphCanvas.Children.Add(edgeViewList.Last().title);
            }

            List<VertexView> vertexViewList = new List<VertexView>();
            List<Vertex> vertexList = _research.getGraph().getVertexList();

            for (int i = 0; i < vertexList.Count; i++)
            {
                vertexViewList.Add(new VertexView(vertexList[i]._x, vertexList[i]._y, vertexList[i]._id));
                this.GraphCanvas.Children.Add(vertexViewList.Last().circle);
                this.GraphCanvas.Children.Add(vertexViewList.Last().text);
            }
        }
    }
}
