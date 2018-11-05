using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Converters {
    /// <summary>
    ///     Returns the container in the specified ItemsControl for the specified item.
    /// </summary>
    /// <remarks>
    ///     <MultiBinding Converter="{StaticResource converter}">
    ///         <Binding ElementName="MainMdiView" />
    ///         <Binding ElementName="MainMdiView" Path="SelectedItem" />
    ///     </MultiBinding>
    /// </remarks>
    public class ContainerFromItemConverter : System.Windows.Data.IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (values.Length != 2)
                throw new ArgumentException("values is expected to contain 2 values: the items control and the item");

            var itemsControl = (System.Windows.Controls.ItemsControl) values[0];
            if (itemsControl == null)
                throw new ArgumentException("The ItemsControl must be specified as the first value and must not be null.");

            var item = values[1];
            if (item != null)
                return itemsControl.ItemContainerGenerator.ContainerFromItem(item);
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}