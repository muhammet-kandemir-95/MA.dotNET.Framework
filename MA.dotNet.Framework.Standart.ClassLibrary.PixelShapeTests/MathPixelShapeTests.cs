using MA.dotNet.Framework.Standart.ClassLibrary.PixelShape;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MA.dotNet.Framework.Standart.ClassLibrary.PixelShapeTests
{
    [TestClass]
    public class MathPixelShapeTests
    {
        [TestMethod]
        public void ConvertionDegreeRadian()
        {
            double[] testDegreeAngles = new double[]
                {
                    0,
                    23,
                    45,
                    65,
                    90,
                    132,
                    135,
                    157,
                    180,
                    210,
                    225,
                    257,
                    270,
                    309,
                    315,
                    333,
                    360
                };
            foreach (var degree in testDegreeAngles)
            {
                var asRadian = MathPixelShape.DegreeToRadian(degree);
                var asDegree = MathPixelShape.RadianToDegree(asRadian);
                Assert.AreEqual(degree, Math.Round(asDegree));
            }
        }

        [TestMethod]
        public void ConvertionWithTestDataDegree()
        {
            Assert.AreEqual(MathPixelShape.DegreeToRadian(23), Math.PI * 23 / 180.0);
        }

        [TestMethod]
        public void ConvertionWithTestDataRadian()
        {
            Assert.AreEqual(MathPixelShape.RadianToDegree(0.40142572795869574d), 0.40142572795869574d * (180.0 / Math.PI));
        }
    }
}
