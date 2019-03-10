using System;
using System.Collections.Generic;
using System.Text;

namespace MA.dotNet.Framework.Standart.ClassLibrary.PixelShape
{
    public static class MathPixelShape
    {
        #region Methods
        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
        #endregion
    }
}
