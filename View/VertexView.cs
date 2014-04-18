using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Kristofides.View
{
    public class VertexView
    {
        public double x, y;
        public int number;
        public Ellipse circle;
        //public TextBox text;
        public Label text;

        public VertexView(double X, double Y, int Number)
        {
            x = X;
            y = Y;
            number = Number;

            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);

            circle = new Ellipse();
            circle.Width = 20;
            circle.Height = 20;
            circle.Margin = new Thickness(x - circle.Width / 2, y - circle.Height / 2, 0, 0);
            circle.Stroke = System.Windows.Media.Brushes.Black;
            circle.StrokeThickness = 2;
            circle.Fill = mySolidColorBrush;
            circle.HorizontalAlignment = HorizontalAlignment.Left;
            circle.VerticalAlignment = VerticalAlignment.Center;
            text = new Label();
            text.Content = (number+1).ToString();
            text.Margin = new Thickness(x - text.Content.ToString().Length*6, y - 13, 0, 0);
        }
    }
}
