using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Media {
    internal class DynamicThickness : System.Windows.DependencyObject {
        public static readonly System.Windows.DependencyProperty LeftProperty = System.Windows.DependencyProperty.Register("Left", typeof(double), typeof(DynamicThickness), new System.Windows.UIPropertyMetadata(0.0, DynamicThickness.OnPropertyChanged));

        public static readonly System.Windows.DependencyProperty TopProperty = System.Windows.DependencyProperty.Register("Top", typeof(double), typeof(DynamicThickness), new System.Windows.UIPropertyMetadata(0.0, DynamicThickness.OnPropertyChanged));

        public static readonly System.Windows.DependencyProperty RightProperty = System.Windows.DependencyProperty.Register("Right", typeof(double), typeof(DynamicThickness), new System.Windows.UIPropertyMetadata(0.0, DynamicThickness.OnPropertyChanged));

        public static readonly System.Windows.DependencyProperty BottomProperty = System.Windows.DependencyProperty.Register("Bottom", typeof(double), typeof(DynamicThickness), new System.Windows.UIPropertyMetadata(0.0, DynamicThickness.OnPropertyChanged));

        private static readonly System.Windows.DependencyPropertyKey ValuePropertyKey = System.Windows.DependencyProperty.RegisterReadOnly("Value", typeof(System.Windows.Thickness), typeof(DynamicThickness), new System.Windows.UIPropertyMetadata(new System.Windows.Thickness(), DynamicThickness.OnPropertyChanged));

        public static readonly System.Windows.DependencyProperty ValueProperty = ValuePropertyKey.DependencyProperty;

        public double Left {
            get => (double) this.GetValue(LeftProperty);
            set => this.SetValue(LeftProperty, value);
        }

        public double Top {
            get => (double) this.GetValue(TopProperty);
            set => this.SetValue(TopProperty, value);
        }

        public double Right {
            get => (double) this.GetValue(RightProperty);
            set => this.SetValue(RightProperty, value);
        }

        public double Bottom {
            get => (double) this.GetValue(BottomProperty);
            set => this.SetValue(BottomProperty, value);
        }

        public System.Windows.Thickness Value {
            get => (System.Windows.Thickness) this.GetValue(BottomProperty);
            private set => this.SetValue(ValuePropertyKey, value);
        }

        private static void OnPropertyChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e) {
            var dt = (DynamicThickness) sender;
            dt.Value = new System.Windows.Thickness(dt.Left, dt.Top, dt.Right, dt.Bottom);
        }
    }
}