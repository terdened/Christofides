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
                    matrixView += matrix[i][j].ToString() + "  ";
                    tempList[i].Add(matrix[i][j]);
                    DataGridRow temp= new DataGridRow();
                   
                   // this.MatrixGrid.Items.Add(matrix[i][j].ToString());
                }
                matrixView += "\n";
            }
            this.MatrixGrid.Content = matrixView;
        }
        
    }
}
