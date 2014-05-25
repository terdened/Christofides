using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
using System.IO;
using System.Xml.Serialization;
using Kristofides.GraphStructure;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Kristofides
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

 
    public partial class MainWindow : Window
    {
        private static Research.Research _research;

        public MainWindow()
        {
            _research = new Research.Research();
            InitializeComponent();
        }

        /// <summary>
        /// Click on menu item "About"
        /// </summary>
        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            String info = "Current version 0.1.8.1";
            MessageBox.Show(info, "About", MessageBoxButton.OK);
        }

        /// <summary>
        /// Click on menu item "Exit"
        /// </summary>
        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to close this window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        
        private void MenuItemSaveGraph_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = String.Format("XML-file|*{0}", "*.xml");
            saveFileDialog.DefaultExt = "xml";
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog(this) == true)
            {
                try
                {
                    File.Delete(saveFileDialog.FileName);
                }
                catch
                {

                }

                XmlSerializer serializer = new XmlSerializer(typeof(Graph));
                TextWriter writer = new StreamWriter(saveFileDialog.FileName);
                serializer.Serialize(writer, _research.getGraph());
                writer.Close();
            }
        }

        private void MenuItemOpenGraph_Click(object sender, RoutedEventArgs e)
        {
           OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = String.Format("Xml-file|*{0}", "*.xml");
            openFileDialog.DefaultExt = "xml";
            openFileDialog.AddExtension = true;

            if (openFileDialog.ShowDialog(this) == true)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Graph));
                TextReader reader = File.OpenText(openFileDialog.FileName);// (openFileDialog.FileName);
                Graph newGraph = (Graph)serializer.Deserialize(reader);
                reader.Close();

                if (newGraph != null)
                {

                    NewResearch();
                    _research.generateGraph(newGraph);
                    _research.getGraph().updateEdgeVertexs();
                    this.MainContent.Content = new ResearchPage(_research, this.isShowMatrix.IsChecked, this.isShowGraph.IsChecked);
                }
            }

        }

        private void NewResearch()
        {
            _research = new Research.Research();

            this.SaveGraphMenuItem.IsEnabled = true;
            this.MainContent.Content = new ResearchPage(_research, false, false);
        }

        private void MenuItemNewGraph_Click(object sender, RoutedEventArgs e)
        {
            string vertexCount = Microsoft.VisualBasic.Interaction.InputBox(
                "Input vertex count", "Input", "3", 100, 100);

            string minEdge = Microsoft.VisualBasic.Interaction.InputBox(
                "Input minimal edge count", "Input", "0", 100, 100);

            string maxEdge = Microsoft.VisualBasic.Interaction.InputBox(
                "Input maximal edge count", "Input", minEdge, 100, 100);

            NewResearch();
            _research.generateGraph(Int32.Parse(vertexCount), Int32.Parse(minEdge), Int32.Parse(maxEdge));
            this.MainContent.Content = new ResearchPage(_research, this.isShowMatrix.IsChecked, this.isShowGraph.IsChecked);
        }


        private void ViewEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.MainContent.Content != null)
            {
                ResearchPage temp = (ResearchPage)this.MainContent.Content;
                temp.updateGraph(this.isShowMatrix.IsChecked, this.isShowGraph.IsChecked);
            }
        }

        private void MenuItemNewGraphConstructor_Click(object sender, RoutedEventArgs e)
        {
            _research = new Research.Research();

            this.SaveGraphConstructorMenuItem.IsEnabled = true;
            this.MainContent.Content = new ConstructorPage(_research);
        }

        private void MenuItemOpenGraphConstructor_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = String.Format("Xml-file|*{0}", "*.xml");
            openFileDialog.DefaultExt = "xml";
            openFileDialog.AddExtension = true;

            if (openFileDialog.ShowDialog(this) == true)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Graph));
                TextReader reader = File.OpenText(openFileDialog.FileName);
                Graph newGraph = (Graph)serializer.Deserialize(reader);
                reader.Close();

                if (newGraph != null)
                {

                    NewResearch();
                    _research.generateGraph(newGraph);
                    _research.getGraph().updateEdgeVertexs();
                    this.MainContent.Content = new ConstructorPage(_research);
                }
            }
        }

    }
}
