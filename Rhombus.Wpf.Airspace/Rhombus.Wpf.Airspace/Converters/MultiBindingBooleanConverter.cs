using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Converters {
    public class MultiBindingBooleanConverter : System.Windows.Data.IMultiValueConverter {
        private bool ConvertToBool(object obj) {
            if (obj is bool)
                return (bool) obj;
            if (obj is bool?)
                return ((bool?) obj).GetValueOrDefault();
            return false;
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            var binaryOp = parameter is string
                ? (string) parameter
                : "";
            if (string.Compare(binaryOp, "and", true) != 0 && string.Compare(binaryOp, "or", true) != 0)
                throw new ArgumentException("MultiBindingBooleanConverter parameter must be either \"and\" or \"or\".");
            var isAnd = string.Compare(binaryOp, "and", true) == 0;
            bool? result = null;

            foreach (var value in values) {
                if (result.HasValue) {
                    // Combine subsequent items.
                    if (isAnd)
                        result &= this.ConvertToBool(value);
                    else
                        result |= this.ConvertToBool(value);
                }
                else {
                    // First time.
                    result = this.ConvertToBool(value);
                }
            }

            return result.GetValueOrDefault();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException("MultiBindingBooleanConverter cannot convert back.");
        }
    }
}