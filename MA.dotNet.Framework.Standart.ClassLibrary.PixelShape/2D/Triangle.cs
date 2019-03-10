using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MA.dotNet.Framework.Standart.ClassLibrary.PixelShape._2D
{
    public struct Triangle : IPixelShape<Triangle>
    {
        #region Constructors
        public Triangle(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.X2 = x2;
            this.Y2 = y2;
            this.X3 = x3;
            this.Y3 = y3;
        }

        public Triangle(PointF point1, PointF point2, PointF point3)
        {
            this.X1 = point1.X;
            this.Y1 = point1.Y;
            this.X2 = point2.X;
            this.Y2 = point2.Y;
            this.X3 = point3.X;
            this.Y3 = point3.Y;
        }

        public Triangle(Point point1, Point point2, Point point3)
        {
            this.X1 = point1.X;
            this.Y1 = point1.Y;
            this.X2 = point2.X;
            this.Y2 = point2.Y;
            this.X3 = point3.X;
            this.Y3 = point3.Y;
        }
        #endregion

        #region Variables
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public double X3 { get; set; }
        public double Y3 { get; set; }
        #endregion

        #region Methods
        public Point[] GetBorderPoints()
        {
            var points1 = new Line(this.X1, this.Y1, this.X2, this.Y2).GetBorderPoints();
            var points2 = new Line(this.X2, this.Y2, this.X3, this.Y3).GetBorderPoints();
            var points3 = new Line(this.X3, this.Y3, this.X1, this.Y1).GetBorderPoints();

            var points = new Point[points1.Length + points2.Length + points3.Length];
            points1.CopyTo(points, 0);
            points2.CopyTo(points, points1.Length);
            points3.CopyTo(points, points1.Length + points2.Length);

            points1 = null;
            points2 = null;
            points3 = null;
            return points;
        }

        public Point[] GetFillPoints()
        {
            throw new NotImplementedException();
        }

        public Triangle Rotate(double originX, double originY, double radian)
        {
            var cosine = Math.Cos(radian);
            var sine = Math.Sin(radian);

            var diffX1 = this.X1 - originX;
            var diffY1 = this.Y1 - originY;
            var diffX2 = this.X2 - originX;
            var diffY2 = this.Y2 - originY;
            var diffX3 = this.X3 - originX;
            var diffY3 = this.Y3 - originY;

            double new_x1AsReal = (diffX1 * cosine - diffY1 * sine) + originX;
            double new_y1AsReal = (diffY1 * cosine + diffX1 * sine) + originY;
            double new_x2AsReal = (diffX2 * cosine - diffY2 * sine) + originX;
            double new_y2AsReal = (diffY2 * cosine + diffX2 * sine) + originY;
            double new_x3AsReal = (diffX3 * cosine - diffY3 * sine) + originX;
            double new_y3AsReal = (diffY3 * cosine + diffX3 * sine) + originY;

            return new Triangle(new_x1AsReal, new_y1AsReal, new_x2AsReal, new_y2AsReal, new_x3AsReal, new_y3AsReal);
        }
        #endregion
    }
}
