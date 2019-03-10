using MA.dotNet.Framework.Standart.ClassLibrary.PixelShape;
using MA.dotNet.Framework.Standart.ClassLibrary.PixelShape._2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace MA.dotNet.Framework.Standart.ClassLibrary.PixelShapeTests._2D
{
    [TestClass]
    public class LineTests
    {
        #region GetFillPoints
        [TestMethod]
        public void GetFillPointsExample1()
        {
            var line = new Line(new Point(0, 0), new Point(3, 3));
            var points = line.GetFillPoints();
            Assert.AreEqual(points.Length, 3);

            Assert.AreEqual(points[0].X, 1);
            Assert.AreEqual(points[0].Y, 1);

            Assert.AreEqual(points[1].X, 2);
            Assert.AreEqual(points[1].Y, 2);

            Assert.AreEqual(points[2].X, 3);
            Assert.AreEqual(points[2].Y, 3);
        }

        [TestMethod]
        public void GetFillPointsExample2()
        {
            var line = new Line(new Point(-2, -2), new Point(2, 2));
            var points = line.GetFillPoints();
            Assert.AreEqual(points.Length, 4);

            Assert.AreEqual(points[0].X, -1);
            Assert.AreEqual(points[0].Y, -1);

            Assert.AreEqual(points[1].X, 0);
            Assert.AreEqual(points[1].Y, 0);

            Assert.AreEqual(points[2].X, 1);
            Assert.AreEqual(points[2].Y, 1);

            Assert.AreEqual(points[3].X, 2);
            Assert.AreEqual(points[3].Y, 2);
        }

        [TestMethod]
        public void GetFillPointsExample3()
        {
            var line = new Line(new Point(0, 0), new Point(5, 2));
            var points = line.GetFillPoints();
            Assert.AreEqual(points.Length, 5);

            Assert.AreEqual(points[0].X, 1);
            Assert.AreEqual(points[0].Y, 0);

            Assert.AreEqual(points[1].X, 2);
            Assert.AreEqual(points[1].Y, 1);

            Assert.AreEqual(points[2].X, 3);
            Assert.AreEqual(points[2].Y, 1);

            Assert.AreEqual(points[3].X, 4);
            Assert.AreEqual(points[3].Y, 2);

            Assert.AreEqual(points[4].X, 5);
            Assert.AreEqual(points[4].Y, 2);
        }

        [TestMethod]
        public void GetFillPointsExample4()
        {
            var line = new Line(new Point(0, 0), new Point(2, 5));
            var points = line.GetFillPoints();
            Assert.AreEqual(points.Length, 5);

            Assert.AreEqual(points[0].X, 0);
            Assert.AreEqual(points[0].Y, 1);

            Assert.AreEqual(points[1].X, 1);
            Assert.AreEqual(points[1].Y, 2);

            Assert.AreEqual(points[2].X, 1);
            Assert.AreEqual(points[2].Y, 3);

            Assert.AreEqual(points[3].X, 2);
            Assert.AreEqual(points[3].Y, 4);

            Assert.AreEqual(points[4].X, 2);
            Assert.AreEqual(points[4].Y, 5);
        }

        [TestMethod]
        public void GetFillPointsExample5()
        {
            var line = new Line(new Point(5, 2), new Point(0, 0));
            var points = line.GetFillPoints();
            Assert.AreEqual(points.Length, 5);

            Assert.AreEqual(points[0].X, 1);
            Assert.AreEqual(points[0].Y, 0);

            Assert.AreEqual(points[1].X, 2);
            Assert.AreEqual(points[1].Y, 1);

            Assert.AreEqual(points[2].X, 3);
            Assert.AreEqual(points[2].Y, 1);

            Assert.AreEqual(points[3].X, 4);
            Assert.AreEqual(points[3].Y, 2);

            Assert.AreEqual(points[4].X, 5);
            Assert.AreEqual(points[4].Y, 2);
        }

        [TestMethod]
        public void GetFillPointsExample6()
        {
            var line = new Line(new Point(2, 5), new Point(0, 0));
            var points = line.GetFillPoints();
            Assert.AreEqual(points.Length, 5);

            Assert.AreEqual(points[0].X, 0);
            Assert.AreEqual(points[0].Y, 1);

            Assert.AreEqual(points[1].X, 1);
            Assert.AreEqual(points[1].Y, 2);

            Assert.AreEqual(points[2].X, 1);
            Assert.AreEqual(points[2].Y, 3);

            Assert.AreEqual(points[3].X, 2);
            Assert.AreEqual(points[3].Y, 4);

            Assert.AreEqual(points[4].X, 2);
            Assert.AreEqual(points[4].Y, 5);
        }
        #endregion

        #region Rotate
        [TestMethod]
        public void RotateExample1()
        {
            var line = new Line(new Point(0, 0), new Point(3, 3));
            var lineAfterRotate = line.Rotate(0, 0, MathPixelShape.DegreeToRadian(45));

            Assert.AreEqual(Math.Ceiling(lineAfterRotate.X1 - 0.5d), 0);
            Assert.AreEqual(Math.Ceiling(lineAfterRotate.Y1 - 0.5d), 0);

            Assert.AreEqual(Math.Ceiling(lineAfterRotate.X2 - 0.5d), 0);
            Assert.AreEqual(lineAfterRotate.Y2.ToString(CultureInfo.InvariantCulture).Substring(0, 12), Math.Sqrt(3 * 3 + 3 * 3).ToString(CultureInfo.InvariantCulture).Substring(0, 12));
        }

        [TestMethod]
        public void RotateExample2()
        {
            var line = new Line(new Point(5, 2), new Point(3, 2));
            var lineAfterRotate = line.Rotate(3, 1, MathPixelShape.DegreeToRadian(90));

            Assert.AreEqual(Math.Ceiling(lineAfterRotate.X1 - 0.5d), 2);
            Assert.AreEqual(Math.Ceiling(lineAfterRotate.Y1 - 0.5d), 3);

            Assert.AreEqual(Math.Ceiling(lineAfterRotate.X2 - 0.5d), 2);
            Assert.AreEqual(Math.Ceiling(lineAfterRotate.Y2 - 0.5d), 1);
        }

        [TestMethod]
        public void RotateExample3()
        {
            var line = new Line(new Point(5, 2), new Point(3, 2));
            var lineAfterRotate = line.Rotate(4, 1, MathPixelShape.DegreeToRadian(90));

            Assert.AreEqual(Math.Ceiling(lineAfterRotate.X1 - 0.5d), 3);
            Assert.AreEqual(Math.Ceiling(lineAfterRotate.Y1 - 0.5d), 2);

            Assert.AreEqual(Math.Ceiling(lineAfterRotate.X2 - 0.5d), 3);
            Assert.AreEqual(Math.Ceiling(lineAfterRotate.Y2 - 0.5d), 0);
        }

        [TestMethod]
        public void RotateExample4()
        {
            var line = new Line(new Point(0, 0), new Point(3, 3));
            var lineAfterRotate = line.Rotate(0, 0, MathPixelShape.DegreeToRadian(-45));

            Assert.AreEqual(Math.Ceiling(lineAfterRotate.X1 - 0.5d), 0);
            Assert.AreEqual(Math.Ceiling(lineAfterRotate.Y1 - 0.5d), 0);

            Assert.AreEqual(lineAfterRotate.X2.ToString(CultureInfo.InvariantCulture).Substring(0, 12), Math.Sqrt(3 * 3 + 3 * 3).ToString(CultureInfo.InvariantCulture).Substring(0, 12));
            Assert.AreEqual(Math.Ceiling(lineAfterRotate.Y2 - 0.5d), 0);
        }

        [TestMethod]
        public void RotateExample5()
        {
            var line = new Line(new Point(0, 0), new Point(3, 3));
            var lineAfterRotate = line.Rotate(0, 0, MathPixelShape.DegreeToRadian(-90));

            Assert.AreEqual(Math.Ceiling(lineAfterRotate.X1 - 0.5d), 0);
            Assert.AreEqual(Math.Ceiling(lineAfterRotate.Y1 - 0.5d), 0);

            Assert.AreEqual(Math.Ceiling(lineAfterRotate.X2 - 0.5d), 3);
            Assert.AreEqual(Math.Ceiling(lineAfterRotate.Y2 - 0.5d), -3);
        }

        [TestMethod]
        public void RotateExample6()
        {
            var line = new Line(new Point(2, 2), new Point(5, 5));
            var lineAfterRotate = line.Rotate(1, 1, MathPixelShape.DegreeToRadian(90));

            Assert.AreEqual(Math.Ceiling(lineAfterRotate.X1 - 0.5d), 0);
            Assert.AreEqual(Math.Ceiling(lineAfterRotate.Y1 - 0.5d), 2);

            Assert.AreEqual(Math.Ceiling(lineAfterRotate.X2 - 0.5d), -3);
            Assert.AreEqual(Math.Ceiling(lineAfterRotate.Y2 - 0.5d), 5);
        }

        [TestMethod]
        public void RotateExample7()
        {
            var line = new Line(new Point(4, 1), new Point(11, 1));
            var lineAfterRotate = line.Rotate(1, 1, MathPixelShape.DegreeToRadian(30));

            Assert.AreEqual(lineAfterRotate.X2.ToString(CultureInfo.InvariantCulture).Substring(0, 12), (1 + (Math.Sqrt(3) * 5)).ToString(CultureInfo.InvariantCulture).Substring(0, 12));
            Assert.AreEqual(Math.Ceiling(lineAfterRotate.Y2 - 0.5d), 6);
        }
        #endregion

        #region IsParallel
        [TestMethod]
        public void IsParallelExample1()
        {
            var line1 = new Line(new Point(1, 1), new Point(1, 4));
            var line2 = new Line(new Point(4, 1), new Point(4, 4));

            Assert.AreEqual(line1.IsParallel(line2), true);
        }

        [TestMethod]
        public void IsParallelExample2()
        {
            var line1 = new Line(new Point(0, 2), new Point(4, 4));
            var line2 = new Line(new Point(1, 0), new Point(5, 2));

            Assert.AreEqual(line1.IsParallel(line2), true);
        }

        [TestMethod]
        public void IsParallelExample3()
        {
            var line1 = new Line(new Point(1, 3), new Point(4, 0));
            var line2 = new Line(new Point(1, 5), new Point(4, 2));

            Assert.AreEqual(line1.IsParallel(line2), true);
        }

        [TestMethod]
        public void IsParallelExample4()
        {
            var line1 = new Line(new Point(10, 1), new Point(10, 4));
            var line2 = new Line(new Point(4, 1), new Point(4, 4));

            Assert.AreEqual(line1.IsParallel(line2), true);
        }

        [TestMethod]
        public void IsParallelExample5()
        {
            var line1 = new Line(new Point(-10, 2), new Point(-6, 4));
            var line2 = new Line(new Point(10, 0), new Point(14, 2));

            Assert.AreEqual(line1.IsParallel(line2), true);
        }

        [TestMethod]
        public void IsParallelExample6()
        {
            var line1 = new Line(new Point(1, 30), new Point(4, 27));
            var line2 = new Line(new Point(1, -12), new Point(4, -15));

            Assert.AreEqual(line1.IsParallel(line2), true);
        }

        [TestMethod]
        public void IsParallelExample7()
        {
            var line1 = new Line(new Point(11, 2), new Point(10, 4));
            var line2 = new Line(new Point(4, 1), new Point(4, 4));

            Assert.AreEqual(line1.IsParallel(line2), false);
        }

        [TestMethod]
        public void IsParallelExample8()
        {
            var line1 = new Line(new Point(-10, 3), new Point(-6, 4));
            var line2 = new Line(new Point(10, 0), new Point(14, 2));

            Assert.AreEqual(line1.IsParallel(line2), false);
        }

        [TestMethod]
        public void IsParallelExample9()
        {
            var line1 = new Line(new Point(1, 30), new Point(4, 27));
            var line2 = new Line(new Point(1, -12), new Point(4, -14));

            Assert.AreEqual(line1.IsParallel(line2), false);
        }
        #endregion

        #region Distance

        #region DistanceByPoint
        [TestMethod]
        public void DistanceExample1()
        {
            var line = new Line(new Point(1, 1), new Point(4, 1));

            Assert.AreEqual(line.DistanceByPoint(new Point2D(3, 3)), 2);
            Assert.AreEqual(line.DistanceByPoint(new Point2D(10, 10)), 9);
        }

        [TestMethod]
        public void DistanceExample2()
        {
            var line = new Line(new Point(-2, -4), new Point(4, 5));

            Assert.AreEqual(line.DistanceByPoint(new Point2D(4, 8)), 1.6641005886756872d);
        }

        [TestMethod]
        public void DistanceExample3()
        {
            var line = new Line(new Point(-6, 1), new Point(0, 3));

            Assert.AreEqual(line.DistanceByPoint(new Point2D(-8, -2)), 2.2135943621178655d);
        }

        [TestMethod]
        public void DistanceExample4()
        {
            var line = new Line(new Point(2, 3), new Point(7, 3));

            Assert.AreEqual(line.DistanceByPoint(new Point2D(12, -2)), 5);
        }
        #endregion

        #region DistanceAsSegment
        [TestMethod]
        public void DistanceAsSegmentExample1()
        {
            var line = new Line(new Point(1, 1), new Point(4, 1));

            Assert.AreEqual(line.DistanceAsSegment(new Point2D(3, 3)), 2);
        }

        [TestMethod]
        public void DistanceAsSegmentExample2()
        {
            var line = new Line(new Point(-2, -4), new Point(4, 5));

            Assert.AreEqual(line.DistanceAsSegment(new Point2D(4, 8)), 3);
        }

        [TestMethod]
        public void DistanceAsSegmentExample3()
        {
            var line = new Line(new Point(-6, 1), new Point(0, 3));

            Assert.AreEqual(line.DistanceAsSegment(new Point2D(-8, -2)), Math.Sqrt(2 * 2 + 3 * 3));
        }

        [TestMethod]
        public void DistanceAsSegmentExample4()
        {
            var line = new Line(new Point(2, 3), new Point(7, 3));

            Assert.AreEqual(line.DistanceAsSegment(new Point2D(12, -2)), Math.Sqrt(5 * 5 + 5 * 5));
        }

        [TestMethod]
        public void DistanceAsSegmentExample5()
        {
            var line = new Line(new Point(-5, 1), new Point(-13, 4));

            Assert.AreEqual(line.DistanceAsSegment(new Point2D(-3, -3)), Math.Sqrt(2 * 2 + 4 * 4));
        }

        [TestMethod]
        public void DistanceAsSegmentExample6()
        {
            var line = new Line(new Point(-13, 4), new Point(-5, 1));

            Assert.AreEqual(line.DistanceAsSegment(new Point2D(-3, -3)), Math.Sqrt(2 * 2 + 4 * 4));
        }

        [TestMethod]
        public void DistanceAsSegmentExample7()
        {
            var line = new Line(new Point(-4, -1), new Point(4, -5));

            Assert.AreEqual(line.DistanceAsSegment(new Point2D(4, -9)), 4);
            Assert.AreEqual(line.DistanceAsSegment(new Point2D(2, -9)), Math.Sqrt(4 * 4 + 2 * 2));
        }

        [TestMethod]
        public void DistanceAsSegmentExample8()
        {
            var line = new Line(new Point(1, 2), new Point(6, 7));

            Assert.AreEqual(line.DistanceAsSegment(new Point2D(2, 7)).ToString(CultureInfo.InvariantCulture).Substring(0, 12), Math.Sqrt(2 * 2 + 2 * 2).ToString(CultureInfo.InvariantCulture).Substring(0, 12));
        }
        #endregion

        #endregion

        #region Intersect

        #region IntersectByPointAsSegment
        [TestMethod]
        public void IntersectByPointAsSegmentExample1()
        {
            var line = new Line(new Point(1, 1), new Point(7, 3));

            Assert.AreEqual(line.IntersectByPointAsSegment(new Point2D(4, 2)), true);
        }

        [TestMethod]
        public void IntersectByPointAsSegmentExample2()
        {
            var line = new Line(new Point(3, -2), new Point(-3, 6));

            Assert.AreEqual(line.IntersectByPointAsSegment(new Point2D(0, 2)), true);
        }

        [TestMethod]
        public void IntersectByPointAsSegmentExample3()
        {
            var line = new Line(new Point(-2, -4), new Point(4, 5));

            Assert.AreEqual(line.IntersectByPointAsSegment(new Point2D(2, 2)), true);
        }

        [TestMethod]
        public void IntersectByPointAsSegmentExample4()
        {
            var line = new Line(new Point(1, 1), new Point(7, 3));

            Assert.AreEqual(line.IntersectByPointAsSegment(new Point2D(4, 3)), false);
        }

        [TestMethod]
        public void IntersectByPointAsSegmentExample5()
        {
            var line = new Line(new Point(3, -2), new Point(-2, 6));

            Assert.AreEqual(line.IntersectByPointAsSegment(new Point2D(0, 2)), false);
        }

        [TestMethod]
        public void IntersectByPointAsSegmentExample6()
        {
            var line = new Line(new Point(-1, -3), new Point(5, 6));

            Assert.AreEqual(line.IntersectByPointAsSegment(new Point2D(2, 2)), false);
        }

        [TestMethod]
        public void IntersectByPointAsSegmentExample7()
        {
            var line = new Line(new Point(1, 1), new Point(4, 4));

            Assert.AreEqual(line.IntersectByPointAsSegment(new Point2D(5, 5)), false);
        }

        [TestMethod]
        public void IntersectByPointAsSegmentExample8()
        {
            var line = new Line(new Point(1, 1), new Point(4, 4));

            Assert.AreEqual(line.IntersectByPointAsSegment(new Point2D(-1, -1)), false);
        }
        #endregion

        #region IntersectByPoint
        [TestMethod]
        public void IntersectByPointExample1()
        {
            var line = new Line(new Point(1, 1), new Point(7, 3));

            Assert.AreEqual(line.IntersectByPoint(new Point2D(4, 2)), true);
        }

        [TestMethod]
        public void IntersectByPointExample2()
        {
            var line = new Line(new Point(3, -2), new Point(-3, 6));

            Assert.AreEqual(line.IntersectByPoint(new Point2D(0, 2)), true);
        }

        [TestMethod]
        public void IntersectByPointExample3()
        {
            var line = new Line(new Point(-2, -4), new Point(4, 5));

            Assert.AreEqual(line.IntersectByPoint(new Point2D(2, 2)), true);
        }

        [TestMethod]
        public void IntersectByPointExample4()
        {
            var line = new Line(new Point(1, 1), new Point(7, 3));

            Assert.AreEqual(line.IntersectByPoint(new Point2D(4, 3)), false);
        }

        [TestMethod]
        public void IntersectByPointExample5()
        {
            var line = new Line(new Point(3, -2), new Point(-2, 6));

            Assert.AreEqual(line.IntersectByPoint(new Point2D(0, 2)), false);
        }

        [TestMethod]
        public void IntersectByPointExample6()
        {
            var line = new Line(new Point(-1, -3), new Point(5, 6));

            Assert.AreEqual(line.IntersectByPoint(new Point2D(2, 2)), false);
        }

        [TestMethod]
        public void IntersectByPointExample7()
        {
            var line = new Line(new Point(1, 1), new Point(4, 4));

            Assert.AreEqual(line.IntersectByPoint(new Point2D(5, 5)), true);
        }

        [TestMethod]
        public void IntersectByPointExample8()
        {
            var line = new Line(new Point(1, 1), new Point(4, 4));

            Assert.AreEqual(line.IntersectByPoint(new Point2D(-1, -1)), true);
        }
        #endregion

        #region IntersectByLineAsSegment
        [TestMethod]
        public void IntersectByLineAsSegmentExample1()
        {
            var line1 = new Line(new Point(1, 1), new Point(4, 3));
            var line2 = new Line(new Point(4, 1), new Point(1, 4));

            Point2D intersectPoint;
            var intersectCheck = line1.IntersectByLineAsSegment(line2, out intersectPoint);

            Assert.AreEqual(intersectCheck, true);
            Assert.AreEqual(intersectPoint.X, 2.8d);
            Assert.AreEqual(intersectPoint.Y, 2.2d);
        }

        [TestMethod]
        public void IntersectByLineAsSegmentExample2()
        {
            var line1 = new Line(new Point(0, 2), new Point(5, 3));
            var line2 = new Line(new Point(1, 0), new Point(4, 5));

            Point2D intersectPoint;
            var intersectCheck = line1.IntersectByLineAsSegment(line2, out intersectPoint);

            Assert.AreEqual(intersectCheck, true);
            Assert.AreEqual(intersectPoint.X, 2.5d);
            Assert.AreEqual(intersectPoint.Y, 2.5d);
        }

        [TestMethod]
        public void IntersectByLineAsSegmentExample3()
        {
            var line1 = new Line(new Point(4, 4), new Point(4, 0));
            var line2 = new Line(new Point(1, 5), new Point(4, 2));

            Point2D intersectPoint;
            var intersectCheck = line1.IntersectByLineAsSegment(line2, out intersectPoint);

            Assert.AreEqual(intersectCheck, true);
            Assert.AreEqual(intersectPoint.X, 4);
            Assert.AreEqual(intersectPoint.Y, 2);
        }

        [TestMethod]
        public void IntersectByLineAsSegmentExample4()
        {
            var line1 = new Line(new Point(0, 0), new Point(5, 1));
            var line2 = new Line(new Point(4, 1), new Point(1, 4));

            Point2D intersectPoint;
            var intersectCheck = line1.IntersectByLineAsSegment(line2, out intersectPoint);

            Assert.AreEqual(intersectCheck, false);
        }

        [TestMethod]
        public void IntersectByLineAsSegmentExample5()
        {
            var line1 = new Line(new Point(0, 2), new Point(3, 5));
            var line2 = new Line(new Point(1, 0), new Point(4, 5));

            Point2D intersectPoint;
            var intersectCheck = line1.IntersectByLineAsSegment(line2, out intersectPoint);

            Assert.AreEqual(intersectCheck, false);
        }

        [TestMethod]
        public void IntersectByLineAsSegmentExample6()
        {
            var line1 = new Line(new Point(4, 4), new Point(4, 0));
            var line2 = new Line(new Point(1, 5), new Point(3, 2));

            Point2D intersectPoint;
            var intersectCheck = line1.IntersectByLineAsSegment(line2, out intersectPoint);

            Assert.AreEqual(intersectCheck, false);
        }

        [TestMethod]
        public void IntersectByLineAsSegmentExample7()
        {
            var line1 = new Line(new Point(1, 1), new Point(6, 6));
            var line2 = new Line(new Point(2, 6), new Point(7, 1));

            Point2D intersectPoint;
            var intersectCheck = line1.IntersectByLineAsSegment(line2, out intersectPoint);

            Assert.AreEqual(intersectCheck, true);
        }

        [TestMethod]
        public void IntersectByLineAsSegmentExample8()
        {
            var line1 = new Line(new Point(1, 1), new Point(3, 3));
            var line2 = new Line(new Point(5, 3), new Point(7, 1));

            Point2D intersectPoint;
            var intersectCheck = line1.IntersectByLineAsSegment(line2, out intersectPoint);

            Assert.AreEqual(intersectCheck, false);
        }
        #endregion

        #endregion
    }
}
