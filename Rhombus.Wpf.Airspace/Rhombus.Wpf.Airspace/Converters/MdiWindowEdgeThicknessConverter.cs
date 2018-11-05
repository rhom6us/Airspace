using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Converters {
    [System.Windows.Data.ValueConversion(typeof(System.Windows.Thickness), typeof(System.Windows.Thickness))]
    public class MdiWindowEdgeThicknessConverter : System.Windows.Data.IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            var thickness = (System.Windows.Thickness) value;
            var edges = (Mdi.MdiWindowEdge) Enum.Parse(typeof(Mdi.MdiWindowEdge), (string) parameter);

            if ((edges & Mdi.MdiWindowEdge.Left) == 0)
                thickness.Left = 0;
            if ((edges & Mdi.MdiWindowEdge.Top) == 0)
                thickness.Top = 0;
            if ((edges & Mdi.MdiWindowEdge.Right) == 0)
                thickness.Right = 0;
            if ((edges & Mdi.MdiWindowEdge.Bottom) == 0)
                thickness.Bottom = 0;
            return thickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return null;
        }
    }
}