using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FillLevelControlTest
{
    public class FillLevelControl : Control
    {
        private FrameworkElement container;
        private Rectangle fillLevelIndicator;

        public FillLevelControl()
        {
            SizeChanged += OnSizeChanged;
        }

        public double FillLevel
        {
            get => (double)GetValue(FillLevelProperty);
            set => SetValue(FillLevelProperty, value);
        }

        public static readonly DependencyProperty FillLevelProperty =
            DependencyProperty.Register("FillLevel", typeof(double), typeof(FillLevelControl), new FrameworkPropertyMetadata(20.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, FillLevelChanged));

        private static void FillLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FillLevelControl)d).OnFillLevelChanged();
        }

        private void OnFillLevelChanged()
        {
            SetFillLevel();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            container = GetTemplateChild("Part_Container") as FrameworkElement;
            fillLevelIndicator = GetTemplateChild("Part_FillLevel") as Rectangle;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetFillLevel();
        }

        private void SetFillLevel()
        {
            if (fillLevelIndicator == null)
            {
                return;
            }

            double fillLevel = FillLevel / 100.0;

            var height = container.ActualHeight * fillLevel;
            fillLevelIndicator.Height = height;

            var fillColor = GetFillColor(fillLevel);
            fillLevelIndicator.Fill = new SolidColorBrush(fillColor);
        }

        private GradientStop[] gradientStops = new GradientStop[] 
        {
            new GradientStop(Color.FromRgb(255, 0, 0), 0),
            new GradientStop(Color.FromRgb(255, 255, 0), 0.4),
            new GradientStop(Color.FromRgb(0, 255, 255), 0.6),
            new GradientStop(Color.FromRgb(0, 255, 0), 1)
        };

        private Color GetFillColor(double fillLevel)
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
