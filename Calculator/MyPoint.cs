using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class MyPoint
    {
        private double x;
        private double y;

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public MyPoint()
        {
            x = 0;
            y = 0;
        }

        public MyPoint(double Xcoord, double Ycoord)
        {
            x = Xcoord;
            y = Ycoord;
        }
    }
}
