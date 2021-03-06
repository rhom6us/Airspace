﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Converters {
    /// <summary>
    ///     A reusable converter for converting between an enum and a bool
    ///     based on the ConverterParameter specifying which enum value is
    ///     selected.
    /// </summary>
    /// <remarks>
    ///     This is useful for displaying an enum using CheckBox or
    ///     RadioButton elements.  See comments below for usage.
    /// </remarks>
    //  <StackPanel> 
    //    <StackPanel.Resources>           
    //      <converters:EnumToBoolConverter x:Key="EnumToBoolConverter" />           
    //    </StackPanel.Resources> 
    //    <RadioButton IsChecked="{Binding Path=YourEnumProperty, Converter={StaticResource EnumToBoolConverter}, ConverterParameter={x:Static local:YourEnumType.Enum1}}" /> 
    //    <RadioButton IsChecked="{Binding Path=YourEnumProperty, Converter={StaticResource EnumToBoolConverter}, ConverterParameter={x:Static local:YourEnumType.Enum2}}" /> 
    //  </StackPanel> 
    public class EnumToBoolConverter : System.Windows.Data.IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return value.Equals(true)
                ? parameter
                : System.Windows.Data.Binding.DoNothing;
        }
    }
}