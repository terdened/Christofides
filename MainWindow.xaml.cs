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

namespace Kristofides
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
 
    public partial class MainWindow : Window
    {
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
            this.Close();
        }
    }
}
