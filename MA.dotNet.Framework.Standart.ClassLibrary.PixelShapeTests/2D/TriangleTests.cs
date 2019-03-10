using MA.dotNet.Framework.Standart.ClassLibrary.PixelShape._2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MA.dotNet.Framework.Standart.ClassLibrary.PixelShapeTests._2D
{
    [TestClass]
    public class TriangleTests
    {
        #region GetBorderPoints
        [TestMethod]
        public void GetBorderPointsExample1()
        {
            var triangle = new Triangle(0, 0, 4, 4, 7, 2);

            var points = triangle.GetBorderPoints();
        }
        #endregion
    }
}
