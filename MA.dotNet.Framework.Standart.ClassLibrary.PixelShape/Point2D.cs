using System;
using System.Collections.Generic;
using System.Text;

namespace MA.dotNet.Framework.Standart.ClassLibrary.PixelShape
{
    public struct Point2D
    {
        #region Constructors
        public Point2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        #endregion

        #region Variables
        public double X { get; set; }
        public double Y { get; set; }
        #endregion

        #region Methods
        public static double DistanceDoublePoint(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public static double DistancePointByOrigin(double x, double y)
        {
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }
        #endregion
    }
}
