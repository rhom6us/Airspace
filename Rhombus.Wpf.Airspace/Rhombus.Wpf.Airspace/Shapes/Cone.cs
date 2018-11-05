using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Shapes {
    public sealed class Cone : ParametricShape3D {
        public static System.Windows.DependencyProperty RadiusProperty = System.Windows.DependencyProperty.Register("Radius", typeof(double), typeof(Cone), new System.Windows.PropertyMetadata(0.5, Shape3D.OnPropertyChangedAffectsModel));

        static Cone() {
            // The height of the cone is specified by MaxV, so make the default
            // MaxV property be 1.
            MaxVProperty.OverrideMetadata(typeof(Cone), new System.Windows.PropertyMetadata(1.0));
        }

        public double Radius {
            get => (double) this.GetValue(RadiusProperty);
            set => this.SetValue(RadiusProperty, value);
        }

        protected override System.Windows.Media.Media3D.Point3D Project(Numerics.MemoizeMath u, Numerics.MemoizeMath v) {
            var radius = this.Radius;
            var height = this.MaxV - this.MinV;

            var x = v.Value * radius * u.Cos / height;
            var y = v.Value * radius * u.Sin / height;
            var z = v.Value;

            return new System.Windows.Media.Media3D.Point3D(x, y, z);
        }
    }
}