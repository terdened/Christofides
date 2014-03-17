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
using System.Collections.ObjectModel;
using Kristofides.GraphStructure;
using Kristofides.View;

namespace Kristofides
{
    /// <summary>
    /// Логика взаимодействия для ResearchPage.xaml
    /// </summary>

    public partial class ResearchPage : Page
    {

        private static Research.Research _research;

        public ResearchPage(Research.Research research)
        {
            InitializeComponent();
            _research = research;
            this.CreateDate.Content = _research.getTimeCreate().ToString();
            this.UpdateDate.Content = _research.getTimeUpdate().ToString();
            ShowMatrix();
            ShowGraph();
        }

        public void ShowMatrix()
        {
            double[][] matrix = _research.getGraph().getMatrix();
            List<List<double>> tempList = new List<List<double>>();
            String matrixView = "";
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                tempList.Add(new List<double>());
                for (int j = 0; j < matrix[i].GetLength(0); j++)
                {
                    matrixView += Math.Round(matrix[i][j]).ToString() + "  ";
                    tempList[i].Add(matrix[i][j]);
                    DataGridRow temp= new DataGridRow();
                   
                   // this.MatrixGrid.Items.Add(matrix[i][j].ToString());
                }
                matrixView += "\n";
            }
            this.MatrixGrid.Content = matrixView;
        }

        public void ShowGraph()
        {
            List<EdgeView> edgeViewList = new List<EdgeView>();
            List<Edge> edgeList = _research.getGraph().getEdgeList();
            for (int i = 0; i < edgeList.Count; i++)
            {
                edgeViewList.Add(new EdgeView(edgeList[i]._a._x, edgeList[i]._a._y, edgeList[i]._b._x, edgeList[i]._b._y, (int)edgeList[i]._length));
                this.GraphCanvas.Children.Add(edgeViewList.Last().line);
                this.GraphCanvas.Children.Add(edgeViewList.Last().text);
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
