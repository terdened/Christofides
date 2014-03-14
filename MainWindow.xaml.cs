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
namespace Kristofides
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

 
    public partial class MainWindow : Window
    {
        private static DataManager.DatabaseManager _db;
        private static Research.Research _research;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Click on menu item "About"
        /// </summary>
        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            String info = "Current version 0.1.0.1";
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

        private void MenuItemNewDatabase_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = String.Format("Database files|*{0}", "*.db");
            saveFileDialog.DefaultExt = "db";
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

                _db = new DataManager.DatabaseManager(saveFileDialog.FileName);
                _db.Create();
                this.ResearchMenuItem.IsEnabled = true;
                this.GraphMenuItem.IsEnabled = false;
                //To do: init MainContent
            }
        }

        private void MenuItemOpenDatabase_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = String.Format("Database files|*{0}", "*.db");
            openFileDialog.DefaultExt = "db";
            openFileDialog.AddExtension = true;

            if (openFileDialog.ShowDialog(this) == true)
            {
                _db = new DataManager.DatabaseManager(openFileDialog.FileName);
                this.ResearchMenuItem.IsEnabled = true;
                this.GraphMenuItem.IsEnabled = false;
                //To do: init MainContent
            }
        }

        private void MenuItemNewResearch_Click(object sender, RoutedEventArgs e)
        {
            _research = new Research.Research();
            this.GraphMenuItem.IsEnabled = true;
            this.MainContent.Content = new ResearchPage();
        }
    }
}
