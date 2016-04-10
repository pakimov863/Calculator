using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Calculator
{
    class PlotParams
    {
        public List<MyPoint> Points;
        public Color PlotColor;

        public PlotParams()
        {
            Points = new List<MyPoint>();
            PlotColor = Color.Black;
        }

        public PlotParams(List<MyPoint> pts, Color clr)
        {
            Points = pts;
            PlotColor = clr;
        }
    }
}
