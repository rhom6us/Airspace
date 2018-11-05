using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Shapes {
    public sealed class MobiusStrip : ParametricShape3D {
        public static System.Windows.DependencyProperty AProperty = System.Windows.DependencyProperty.Register("A", typeof(double), typeof(MobiusStrip), new System.Windows.PropertyMetadata(1.0, Shape3D.OnPropertyChangedAffectsModel));

        static MobiusStrip() {
            // MinV should be -1.
            MinVProperty.OverrideMetadata(typeof(MobiusStrip), new System.Windows.PropertyMetadata(-1.0));

            // MaxV should be 1.
            MaxVProperty.OverrideMetadata(typeof(MobiusStrip), new System.Windows.PropertyMetadata(1.0));
        }

        public double A {
            get => (double) this.GetValue(AProperty);
            set => this.SetValue(AProperty, value);
        }

        protected override System.Windows.Media.Media3D.Point3D Project(Numerics.MemoizeMath u, Numerics.MemoizeMath v) {
            var a = this.A;

            var x = (a - v.Value * Math.Sin(u.Value / 2.0)) * u.Sin;
            var y = (a - v.Value * Math.Sin(u.Value / 2.0)) * u.Cos;
            var z = v.Value * Math.Cos(u.Value / 2.0);
            return new System.Windows.Media.Media3D.Point3D(x, y, z);
        }
    }
}