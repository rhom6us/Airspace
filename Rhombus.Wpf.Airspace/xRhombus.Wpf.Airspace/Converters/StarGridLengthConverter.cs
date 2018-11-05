using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Converters {
    [System.Windows.Data.ValueConversion(typeof(double), typeof(System.Windows.GridLength))]
    public class StarGridLengthConverter : System.Windows.Data.IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return new System.Windows.GridLength((double) value, System.Windows.GridUnitType.Star);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return ((System.Windows.GridLength) value).Value;
        }
    }
}