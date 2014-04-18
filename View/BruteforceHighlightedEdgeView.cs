using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;

namespace Kristofides.View
{
    public class BruteforceHighlightedEdgeView
    {
        public double x1, y1, x2, y2;
        public int length;
        public Line line;
        public Label text;

        public BruteforceHighlightedEdgeView(double X1, double Y1, double X2, double Y2, int Length)
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
            line.Stroke = System.Windows.Media.Brushes.Red;
            line.StrokeThickness = 4;

            text = new Label();
            text.Content = length.ToString();
            text.Margin = new Thickness(-text.Content.ToString().Length*9 + (x1 + x2) / 2, -9 + (y1 + y2) / 2, 0, 0);
        }
    }
}
