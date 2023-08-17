using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FillLevelControlTest
{
    public class FillLevelControl : RangeBase
    {
        private FrameworkElement container;
        private Rectangle fillLevelIndicator;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (container != null)
            {
                container.SizeChanged -= OnContainerSizeChanged;
            }

            container = GetTemplateChild("Part_Container") as FrameworkElement;
            fillLevelIndicator = GetTemplateChild("Part_FillLevel") as Rectangle;

            container.SizeChanged += OnContainerSizeChanged;
        }

        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            SetFillLevel();
        }

        protected override void OnMaximumChanged(double oldMinimum, double newMinimum)
        {
            SetFillLevel();
        }

        protected override void OnValueChanged(double oldMinimum, double newMinimum)
        {
            SetFillLevel();
        }

        private void OnContainerSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetFillLevel();
        }

        private void SetFillLevel()
        {
            if (container == null || fillLevelIndicator == null)
            {
                return;
            }

            double fillLevel = (Value - Minimum) / (Maximum - Minimum);

            var height = container.ActualHeight * fillLevel;
            fillLevelIndicator.Height = height;

            var fillColor = GetGradientColor(fillLevel);
            fillLevelIndicator.Fill = new SolidColorBrush(fillColor);
        }

        // the array must be sorted ascending by offset
        private GradientStop[] gradientStops = new GradientStop[]
        {
            new GradientStop(Color.FromRgb(255, 0, 0), 0),
            new GradientStop(Color.FromRgb(255, 255, 0), 0.4),
            new GradientStop(Color.FromRgb(0, 255, 255), 0.6),
            new GradientStop(Color.FromRgb(0, 255, 0), 1)
        };

        private Color GetGradientColor(double fillLevel)
        {
            int lastStop = gradientStops.Count() - 1;

            if (lastStop < 0)
            {
                return Colors.Transparent;
            }

            if (fillLevel >= gradientStops[lastStop].Offset)
            {
                return gradientStops[lastStop].Color;
            }

            if (fillLevel <= gradientStops[0].Offset)
            {
                return gradientStops[0].Color;
            }

            int index = 0;

            while (fillLevel > gradientStops[index + 1].Offset)
            {
                index++;
            }

            var fl = (fillLevel - gradientStops[index].Offset) / (gradientStops[index + 1].Offset - gradientStops[index].Offset);
            var nfl = 1 - fl;

            Color color1 = gradientStops[index + 1].Color;
            Color color2 = gradientStops[index].Color;

            byte r = (byte)(color1.R * fl + color2.R * nfl);
            byte g = (byte)(color1.G * fl + color2.G * nfl);
            byte b = (byte)(color1.B * fl + color2.B * nfl);

            return Color.FromRgb(r, g, b);
        }
    }
}
