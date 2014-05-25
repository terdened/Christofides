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
    public class EdgeView
    {
        public double x1, y1, x2, y2;
        public int length;
        public Line line;
        public Label text;
        public Label title;

        public EdgeView(double X1, double Y1, double X2, double Y2, int Length,string Title)
        {
            x1 = X1;
            y1 = Y1;
            x2 = X2;
            y2 = Y2;
            length = Length;

            line = new Line();
            line.X1 = x1;
            line.X2 = x2;
            line.Y1 = y1;
            line.Y2 = y2;
            line.Stroke = System.Windows.Media.Brushes.Black;
            line.StrokeThickness = 1;

            text = new Label();
            text.Content = length.ToString();
            text.Margin = new Thickness(-text.Content.ToString().Length*9 + (x1 + x2) / 2, -9 + (y1 + y2) / 2, 0, 0);
            double tan = 0;
            if ((x1 - x2) != 0)
            {
                tan = (y1 - y2) / (x1 - x2);
            }
            else
            {
                tan = (y1 - y2) / 0.0001;
            }

            double angle = Math.Atan(tan) * 180 / Math.PI;
            RotateTransform rotateTransform1 = new RotateTransform(angle, text.Content.ToString().Length * 9 / 2, 0);
            text.RenderTransform = rotateTransform1;
            text.Foreground = System.Windows.Media.Brushes.Brown;


            title = null;
            if (Title != "")
            {
                title = new Label();
                title.Content = Title;

                title.Margin = new Thickness(-title.Content.ToString().Length * 9 + (x1 + x2) / 2, 10 + (y1 + y2) / 2, 0, 0);
                RotateTransform rotateTransform2 = new RotateTransform(angle, title.Content.ToString().Length * 9 / 2, 0);
                title.RenderTransform = rotateTransform2;
                title.Foreground = System.Windows.Media.Brushes.DarkGreen;
            }
        }
    }
}
