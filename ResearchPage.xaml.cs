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
using Kristofides.GraphSolver;
using Kristofides.View;
using Kristofides.WidthSearchOptimization;

namespace Kristofides
{
    /// <summary>
    /// Логика взаимодействия для ResearchPage.xaml
    /// </summary>

    public partial class ResearchPage : Page
    {

        #region private paramrters
        private static Research.Research _research;
        private Boolean isShowMatrix;
        private Boolean isShowGraph;
        private List<HighlightedEdgeView> highlightedEdgeViewList;
        private List<BruteforceHighlightedEdgeView> bruteforceHighlightedEdgeViewList;
        private GraphSolver.KristofidesSolver kristofidesSolution;
        private List<List<int>> loopControl;
        private int maxLoopControlDeep = 8;
        private int minLoopControlDeep = 3;
        #endregion

        #region public methods
        public ResearchPage(Research.Research research, Boolean isShowMatrix, Boolean isShowGraph)
        {
            InitializeComponent();
            _research = research;
            this.CreateDate.Content = _research.getTimeCreate().ToString();
            this.UpdateDate.Content = _research.getTimeUpdate().ToString();

            updateGraph(isShowMatrix, isShowGraph);
        }

        public void updateGraph(Boolean isShowMatrix, Boolean isShowGraph)
        {
            this.isShowGraph = isShowGraph;
            this.isShowMatrix = isShowMatrix;

            if (isShowMatrix)
            {
                if (_research.getGraph()!=null)
                    ShowMatrix();
            }
            else
            {
                this.MatrixGrid.Content = "";
            }

            if (isShowGraph)
            {
                if (_research.getGraph() != null)
                    ShowGraph();
            }
            else
            {
                this.GraphCanvas.Children.Clear();
            }
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


            kristofidesSolution = new GraphSolver.KristofidesSolver(_research.getGraph());
            loopControl = new List<List<int>>();
        }
        #endregion

        #region private methods
        private void HighlightGraph(List<Edge> edges)
        {
            ClearHiglitedEdges();
            this.highlightedEdgeViewList = new List<HighlightedEdgeView>();
            for (int i = 0; i < edges.Count; i++)
            {
                highlightedEdgeViewList.Add(new HighlightedEdgeView(edges[i]._a._x, edges[i]._a._y, edges[i]._b._x, edges[i]._b._y, (int)edges[i]._length));
                this.GraphCanvas.Children.Add(highlightedEdgeViewList.Last().line);
                //this.GraphCanvas.Children.Add(highlightedEdgeViewList.Last().text);
            }
        }

        private void ClearHiglitedEdges()
        {
            if (highlightedEdgeViewList != null)
            {
                for (int i = 0; i < highlightedEdgeViewList.Count; i++)
                {
                    this.GraphCanvas.Children.Remove(highlightedEdgeViewList[i].line);
                    //this.GraphCanvas.Children.Remove(highlightedEdgeViewList[i].text);
                }
            }
        }

        private void BruteforceHighlightGraph(List<Edge> edges)
        {
            BruteforceHighlightGraph();
            this.bruteforceHighlightedEdgeViewList = new List<BruteforceHighlightedEdgeView>();
            for (int i = 0; i < edges.Count; i++)
            {
                bruteforceHighlightedEdgeViewList.Add(new BruteforceHighlightedEdgeView(edges[i]._a._x, edges[i]._a._y, edges[i]._b._x, edges[i]._b._y, (int)edges[i]._length));
                this.GraphCanvas.Children.Add(bruteforceHighlightedEdgeViewList.Last().line);
            }
        }

        private void BruteforceHighlightGraph()
        {
            if (bruteforceHighlightedEdgeViewList != null)
            {
                for (int i = 0; i < bruteforceHighlightedEdgeViewList.Count; i++)
                    this.GraphCanvas.Children.Remove(bruteforceHighlightedEdgeViewList[i].line);
            }
        }

        private void updateBackup(Graph modified)
        {
            ListBoxItem newItem = new ListBoxItem();
            Graph skeleton = kristofidesSolution.GetSkeleton(modified);

            List<int> tempVertexesDegree = new List<int>();
            string title = "";
            foreach (Vertex vertex in skeleton._vertexList)
            {
                tempVertexesDegree.Add(vertex._edgeList.Count);
                title += vertex._edgeList.Count.ToString();
            }

            if (loopControl.Count > 0)
            {
                if (!IsCompareTwoRecords(tempVertexesDegree, loopControl.Last()))
                {
                    newItem.Content = title;
                    BackupList.Items.Add(newItem);
                    loopControl.Add(tempVertexesDegree);
                }
            }
            else
            {
                newItem.Content = title;
                BackupList.Items.Add(newItem);
                loopControl.Add(tempVertexesDegree);
            }

            if(IsLooped())
                MessageBox.Show("Loop");
        }

        private bool IsCompareTwoRecords(List<int> a, List<int> b)
        {
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }

        private bool IsLooped()
        {
            int deep = maxLoopControlDeep;
            if (loopControl.Count < deep * 2)
                deep = loopControl.Count / 2;

            if (deep >= minLoopControlDeep)
            {
                for (int i = minLoopControlDeep-1; i < deep; i++)
                {
                    List<List<int>> set = new List<List<int>>();
                    for (int j = 0; j < i + 1; j++)
                    {
                        set.Add(loopControl[loopControl.Count - 1 - j]);
                    }

                    bool isCompare = true;

                    for (int j = 0; j < set.Count; j++)
                    {
                        List<int> item2 = loopControl[loopControl.Count - 1 - (j+set.Count)];
                        if (!IsCompareTwoRecords(set[j], item2))
                        {
                            isCompare = false;
                            break;
                        }
                    }

                    if (isCompare)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsSolved(Graph skeleton)
        {
            bool result = true;

            if (skeleton != null)
            {
                foreach (Vertex vertex in skeleton._vertexList)
                {
                    if (vertex._edgeList.Count > 2)
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                result = false;
            }
        

            if (result)
                MessageBox.Show("Solved");

            return result;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            GraphSolver.Brutforce sol = new GraphSolver.Brutforce(_research.getGraph());
            List<Vertex> result=sol.Solve();
            List<Edge> bruteForceResult = new List<Edge>();

           

            string answer = "";

            if (result != null)
            {
                for (int i = 0; i < result.Count - 1; i++)
                {
                    bruteForceResult.Add(new Edge(result[i], result[i + 1]));
                }

                BruteforceHighlightGraph(bruteForceResult);

                foreach (Vertex vertex in result)
                {
                    answer += (vertex._id + 1) + "; ";
                }
            }
            else
            {
                answer = "There is no solution!";
            }
            MessageBox.Show(answer);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if(kristofidesSolution==null)
                kristofidesSolution = new GraphSolver.KristofidesSolver(_research.getGraph());

            kristofidesSolution.Reset();
            List<Edge> edges = kristofidesSolution.Solve()._edgeList;
            HighlightGraph(edges);
            BackupList.Items.Clear();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            
            WidthSearchOptimizator optimizator = new WidthSearchOptimizator();
            loopControl = new List<List<int>>();

            string report = optimizator.Iteration(kristofidesSolution);
            ListBoxItem lbi = new ListBoxItem();
            lbi.Content = report;
            BackupList.Items.Add(lbi);

            //kristofidesSolution.SetModified(input);
            HighlightGraph(kristofidesSolution.Solve()._edgeList);
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            string penaltyValue = Microsoft.VisualBasic.Interaction.InputBox(
                "Input penalty value", "Input", "5", 100, 100);

            if (kristofidesSolution != null)
            {
                kristofidesSolution.PositivePenalty(double.Parse(penaltyValue));
                List<Edge> edges = kristofidesSolution.Solve()._edgeList;
                HighlightGraph(edges);
            }

            updateBackup(kristofidesSolution.GetModified());
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            double penaltyValue = 10;

            if (kristofidesSolution != null)
            {
                kristofidesSolution.PositivePenalty(penaltyValue);
                List<Edge> edges = kristofidesSolution.GetSkeleton(kristofidesSolution.GetModified())._edgeList;
                HighlightGraph(edges);
            }

            updateBackup(kristofidesSolution.GetModified());
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            string penaltyValue = Microsoft.VisualBasic.Interaction.InputBox(
                "Input penalty value", "Input", "5", 100, 100);

            if (kristofidesSolution != null)
            {
                loopControl.Clear();
                BackupList.Items.Clear();
                Graph skeleton = null;
                while (!IsSolved(skeleton) && !IsLooped())
                {
                    kristofidesSolution.PositivePenalty(double.Parse(penaltyValue));
                    List<Edge> edges = kristofidesSolution.Solve()._edgeList;
                    HighlightGraph(edges);
                    updateBackup(kristofidesSolution.GetModified());
                }
            }

        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            string penaltyValue = Microsoft.VisualBasic.Interaction.InputBox(
                "Input penalty value", "Input", "5", 100, 100);

            if (kristofidesSolution != null)
            {
                kristofidesSolution.NegativePenalty(double.Parse(penaltyValue));
                List<Edge> edges = kristofidesSolution.Solve()._edgeList;
                HighlightGraph(edges);
            }

            updateBackup(kristofidesSolution.GetModified());
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            double penaltyValue = 10;

            if (kristofidesSolution != null)
            {
                kristofidesSolution.NegativePenalty(penaltyValue);
                List<Edge> edges = kristofidesSolution.Solve()._edgeList;
                HighlightGraph(edges);
            }

            updateBackup(kristofidesSolution.GetModified());
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            string penaltyValue = Microsoft.VisualBasic.Interaction.InputBox(
                "Input penalty value", "Input", "5", 100, 100);

            if (kristofidesSolution != null)
            {

                loopControl.Clear();
                BackupList.Items.Clear();
                Graph skeleton = null;
                while (!IsSolved(skeleton) && !IsLooped())
                {
                    kristofidesSolution.NegativePenalty(double.Parse(penaltyValue));
                    List<Edge> edges = kristofidesSolution.Solve()._edgeList;
                    HighlightGraph(edges);
                    updateBackup(kristofidesSolution.GetModified());
                }
            }
        }

        private void button10_Click(object sender, RoutedEventArgs e)
        {
            string penaltyValue = Microsoft.VisualBasic.Interaction.InputBox(
                "Input penalty value", "Input", "5", 100, 100);

            if (kristofidesSolution != null)
            {
                kristofidesSolution.CombinePenalty(double.Parse(penaltyValue));
                List<Edge> edges = kristofidesSolution.Solve()._edgeList;
                HighlightGraph(edges);
            }

            updateBackup(kristofidesSolution.GetModified());
        }

        private void button11_Click(object sender, RoutedEventArgs e)
        {
            double penaltyValue = 10;

            if (kristofidesSolution != null)
            {
                kristofidesSolution.CombinePenalty(penaltyValue);
                List<Edge> edges = kristofidesSolution.Solve()._edgeList;
                HighlightGraph(edges);
            }

            updateBackup(kristofidesSolution.GetModified());
        }

        private void button12_Click(object sender, RoutedEventArgs e)
        {
            string penaltyValue = Microsoft.VisualBasic.Interaction.InputBox(
                "Input penalty value", "Input", "5", 100, 100);

            if (kristofidesSolution != null)
            {
                loopControl.Clear();
                BackupList.Items.Clear();
                Graph skeleton = null;
                while (!IsSolved(skeleton) && !IsLooped())
                {
                    kristofidesSolution.CombinePenalty(double.Parse(penaltyValue));
                    List<Edge> edges = kristofidesSolution.Solve()._edgeList;
                    HighlightGraph(edges);
                    updateBackup(kristofidesSolution.GetModified());
                }
            }
        }
        #endregion
    }
}
