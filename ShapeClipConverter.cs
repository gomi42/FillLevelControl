using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FillLevelControlTest
{
    internal class ShapeClipConverter : IValueConverter
    {
        /// The aspect ratio (height/width) of the geometry
        /// </summary>
        public const double AspectRatio = 1.11333990237159;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return CreateFromHeight((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create the geometry from the given height and keep the original aspect ratio
        /// Shapes extracted from file "D:\Programme\Tools\FillLevelControlTest\ComplexShape.svg"
        /// Generated from the following stream geometry:
        /// F0M182.85714,213.94826 L574.28572,116.8054 L679.99997,488.23397 L491.42857,373.94826 L437.14284,776.80539 L108.57143,719.66253 L385.71428,388.23398 z
        /// </summary>
        /// <param name="height">the height in WPF units</param>
        /// <returns>Returns the geometry</returns>
        private Geometry CreateFromHeight(double height)
        {
            double scale = height;
            StreamGeometry geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;
            StreamGeometryContext ctx = geometry.Open();

            ctx.BeginFigure(new Point(0.11255 * scale, 0.14719 * scale), true, true);
            ctx.LineTo(new Point(0.70563 * scale, 0), true, true);
            ctx.LineTo(new Point(0.8658 * scale, 0.56277 * scale), true, true);
            ctx.LineTo(new Point(0.58009 * scale, 0.38961 * scale), true, true);
            ctx.LineTo(new Point(0.49784 * scale, scale), true, true);
            ctx.LineTo(new Point(0, 0.91342 * scale), true, true);
            ctx.LineTo(new Point(0.41991 * scale, 0.41126 * scale), true, true);

            ctx.Close();
            geometry.Freeze();

            return geometry;
        }
    }
}
