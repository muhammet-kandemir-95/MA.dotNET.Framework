using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MA.dotNet.Framework.Standart.ClassLibrary.PixelShape._2D
{
    public interface IPixelShape
    {
        #region Variables

        #endregion

        #region Methods
        Point[] GetBorderPoints();
        Point[] GetFillPoints();
        #endregion
    }

    public interface IPixelShape<T> : IPixelShape where T : IPixelShape<T>
    {
        #region Variables

        #endregion

        #region Methods
        T Rotate(double originX, double originY, double radian);
        #endregion
    }
}
