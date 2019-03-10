using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MA.dotNet.Framework.Standart.ClassLibrary.PixelShape._2D
{
    public struct Line : IPixelShape<Line>
    {
        #region Constructors
        public Line(double x1, double y1, double x2, double y2)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.X2 = x2;
            this.Y2 = y2;
        }

        public Line(PointF point1, PointF point2)
        {
            this.X1 = point1.X;
            this.Y1 = point1.Y;
            this.X2 = point2.X;
            this.Y2 = point2.Y;
        }

        public Line(Point point1, Point point2)
        {
            this.X1 = point1.X;
            this.Y1 = point1.Y;
            this.X2 = point2.X;
            this.Y2 = point2.Y;
        }
        #endregion

        #region Variables
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        #endregion

        #region Methods
        public Point[] GetBorderPoints()
        {
            Point[] result = null;

            #region Calculate

            double minX = Math.Min(this.X1, this.X2);
            double maxX = Math.Max(this.X1, this.X2);
            double minY = Math.Min(this.Y1, this.Y2);
            double maxY = Math.Max(this.Y1, this.Y2);

            double diffX = maxX - minX;
            double diffY = maxY - minY;

            double hypotenuse = Math.Sqrt((diffX * diffX) + (diffY * diffY));
            double cosine = diffX / hypotenuse;
            double sine = diffY / hypotenuse;

            if (diffX > diffY)
            {
                double cotangent = cosine / sine;
                int diffXAsInteger = (int)(Math.Ceiling(diffX - 0.555555d));
                result = new Point[diffXAsInteger];

                var minXisX1 = minX == this.X1;
                var minX_Y = minXisX1 == true ? this.Y1 : this.Y2;
                var minX_YisYMinY = minY == minX_Y;

                var addY = minX_YisYMinY == true ? minY : maxY;
                var combine = minX_YisYMinY == true ? 1 : -1;

                for (int x = 1; x <= diffXAsInteger; x++)
                {
                    double yAsReal = x / cotangent;
                    int yAsInteger = (int)Math.Ceiling(yAsReal - 0.555555d);
                    result[x - 1] = new Point(x + (int)(Math.Ceiling(minX - 0.555555d)), (yAsInteger * combine) + (int)(Math.Ceiling(addY - 0.555555d)));
                }
            }
            else
            {
                double tangent = sine / cosine;
                int diffYAsInteger = (int)(Math.Ceiling(diffY - 0.555555d));

                var minYisY1 = minY == this.Y1;
                var minY_X = minYisY1 == true ? this.X1 : this.X2;
                var minY_XisXMinX = minY == minY_X;

                var addX = minY_XisXMinX == true ? minX : maxX;
                var combine = minY_XisXMinX == true ? 1 : -1;

                result = new Point[diffYAsInteger];
                for (int y = 1; y <= diffYAsInteger; y++)
                {
                    double xAsReal = y / tangent;
                    int xAsInteger = (int)Math.Ceiling(xAsReal - 0.555555d);
                    result[y - 1] = new Point((xAsInteger * combine) + (int)(Math.Ceiling(addX - 0.555555d)), y  + (int)(Math.Ceiling(minY - 0.555555d)));
                }
            }

            #endregion

            return result;
        }

        public Point[] GetFillPoints()
        {
            return GetBorderPoints();
        }

        public Line Rotate(double originX, double originY, double radian)
        {
            var cosine = Math.Cos(radian);
            var sine = Math.Sin(radian);

            var diffX1 = this.X1 - originX;
            var diffY1 = this.Y1 - originY;
            var diffX2 = this.X2 - originX;
            var diffY2 = this.Y2 - originY;

            double new_x1AsReal = (diffX1 * cosine - diffY1 * sine) + originX;
            double new_y1AsReal = (diffY1 * cosine + diffX1 * sine) + originY;
            double new_x2AsReal = (diffX2 * cosine - diffY2 * sine) + originX;
            double new_y2AsReal = (diffY2 * cosine + diffX2 * sine) + originY;

            return new Line(new_x1AsReal, new_y1AsReal, new_x2AsReal, new_y2AsReal);
        }

        public bool IsParallel(Line line)
        {
            return (this.X1 - this.X2) * (line.Y1 - line.Y2) - (this.Y1 - this.Y2) * (line.X1 - line.X2) == 0;
        }

        public double Distance()
        {
            return Point2D.DistanceDoublePoint(this.X1, this.Y1, this.X2, this.Y2);
        }

        internal static double DistanceByPoint(Line line, Point2D point)
        {
            return (Math.Abs((line.Y2 - line.Y1) * point.X - (line.X2 - line.X1) * point.Y + line.X2 * line.Y1 - line.Y2 * line.X1)) / (Point2D.DistanceDoublePoint(line.X1, line.Y1, line.X2, line.Y2));
        }

        public double DistanceByPoint(Point2D point)
        {
            return DistanceByPoint(this, point);
        }

        public double DistanceAsSegment(Point2D point)
        {
            double diffX = this.X2 - this.X1;
            double diffY = this.Y2 - this.Y1;

            double hypotenuse = Math.Sqrt((diffX * diffX) + (diffY * diffY));
            double cosine = diffX / hypotenuse;
            double sine = diffY / hypotenuse;

            var distance1 = Math.Abs(cosine * (point.X - this.X1) + sine * (point.Y - this.Y1));
            var distance2 = Math.Abs(cosine * (point.X - this.X2) + sine * (point.Y - this.Y2));

            if (Math.Round(distance1 + distance2, 6) == Math.Round(hypotenuse, 6))
                return DistanceByPoint(point);

            return Math.Min(
                Point2D.DistanceDoublePoint(point.X, point.Y, this.X1, this.Y1),
                Point2D.DistanceDoublePoint(point.X, point.Y, this.X2, this.Y2)
                );
        }

        public bool IntersectByPoint(Point2D point)
        {
            return (point.X - this.X1) / (this.X2 - this.X1) == (point.Y - this.Y1) / (this.Y2 - this.Y1);
        }

        public bool IntersectByPointAsSegment(Point2D point)
        {
            return IntersectByPoint(point) && (Math.Round(Distance(), 6) == Math.Round(Point2D.DistanceDoublePoint(point.X, point.Y, this.X1, this.Y1) + Point2D.DistanceDoublePoint(point.X, point.Y, this.X2, this.Y2), 6));
        }

        public bool IntersectByLineAsSegment(Line line, out Point2D intersectPoint)
        {
            intersectPoint = new Point2D();

            double t = ((this.X1 - line.X1) * (line.Y1 - line.Y2) - (this.Y1 - line.Y1) * (line.X1 - line.X2)) / ((this.X1 - this.X2) * (line.Y1 - line.Y2) - (this.Y1 - this.Y2) * (line.X1 - line.X2));
            double u = -1 * ((this.X1 - this.X2) * (this.Y1 - line.Y1) - (this.Y1 - this.Y2) * (this.X1 - line.X1)) / ((this.X1 - this.X2) * (line.Y1 - line.Y2) - (this.Y1 - this.Y2) * (line.X1 - line.X2));

            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                intersectPoint = new Point2D(this.X1 + t * (this.X2 - this.X1), this.Y1 + t * (this.Y2 - this.Y1));

                return true;
            }

            return false;
        }
        #endregion
    }
}
