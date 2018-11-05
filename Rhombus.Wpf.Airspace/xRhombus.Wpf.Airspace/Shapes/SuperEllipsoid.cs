using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Shapes {
    public class SuperEllipsoid : ParametricShape3D {
        public static System.Windows.DependencyProperty N1Property = System.Windows.DependencyProperty.Register("N1", typeof(double), typeof(SuperEllipsoid), new System.Windows.PropertyMetadata(4.0, Shape3D.OnPropertyChangedAffectsModel));

        public static System.Windows.DependencyProperty N2Property = System.Windows.DependencyProperty.Register("N2", typeof(double), typeof(SuperEllipsoid), new System.Windows.PropertyMetadata(4.0, Shape3D.OnPropertyChangedAffectsModel));

        public double N1 {
            get => (double) this.GetValue(N1Property);
            set => this.SetValue(N1Property, value);
        }

        public double N2 {
            get => (double) this.GetValue(N2Property);
            set => this.SetValue(N2Property, value);
        }

        protected override System.Windows.Media.Media3D.Point3D Project(Numerics.MemoizeMath u, Numerics.MemoizeMath v) {
            var xRadius = 1.0;
            var yRadius = 1.0;
            var zRadius = 1.0;
            var n1 = this.N1;
            var n2 = this.N2;

            var x = xRadius * SuperEllipsoid.SafePow(v.Sin, n1) * SuperEllipsoid.SafePow(u.Cos, n2);
            var y = yRadius * SuperEllipsoid.SafePow(v.Sin, n1) * SuperEllipsoid.SafePow(u.Sin, n2);
            var z = zRadius * SuperEllipsoid.SafePow(v.Cos, n1);
            return new System.Windows.Media.Media3D.Point3D(x, y, z);
        }

        private static double SafePow(double value, double power) {
            if (value > 0)
                return Math.Pow(value, power);
            if (value < 0)
                return -Math.Pow(-value, power);
            return 0;
        }
    }
}