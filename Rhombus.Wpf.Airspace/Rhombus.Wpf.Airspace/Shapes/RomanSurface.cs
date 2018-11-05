using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Shapes {
    public sealed class RomanSurface : ParametricShape3D {
        public static System.Windows.DependencyProperty AProperty = System.Windows.DependencyProperty.Register("A", typeof(double), typeof(RomanSurface), new System.Windows.PropertyMetadata(1.0, Shape3D.OnPropertyChangedAffectsModel));

        public double A {
            get => (double) this.GetValue(AProperty);
            set => this.SetValue(AProperty, value);
        }

        protected override System.Windows.Media.Media3D.Point3D Project(Numerics.MemoizeMath u, Numerics.MemoizeMath v) {
            var a = this.A;

            var x = a * a * Math.Sin(2.0 * u.Value) * Math.Pow(v.Cos, 2) / 2.0;
            var y = a * a * u.Sin * Math.Sin(v.Value * 2.0) / 2.0;
            var z = a * a * u.Cos * Math.Sin(v.Value * 2.0) / 2.0;
            return new System.Windows.Media.Media3D.Point3D(x, y, z);
        }
    }
}