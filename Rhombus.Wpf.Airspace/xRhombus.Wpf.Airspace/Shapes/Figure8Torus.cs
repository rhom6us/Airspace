using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Shapes {
    public class Figure8Torus : ParametricShape3D {
        public static System.Windows.DependencyProperty AProperty = System.Windows.DependencyProperty.Register("A", typeof(double), typeof(Figure8Torus), new System.Windows.PropertyMetadata(1.0, Shape3D.OnPropertyChangedAffectsModel));

        static Figure8Torus() {
            // So texture coordinates work out better, configure the default
            // MinV property to be PI.
            MinVProperty.OverrideMetadata(typeof(Figure8Torus), new System.Windows.PropertyMetadata(Math.PI));

            // So texture coordinates work out better, configure the default
            // MaxV property to be 3*PI.
            MaxVProperty.OverrideMetadata(typeof(Figure8Torus), new System.Windows.PropertyMetadata(Math.PI * 3.0));
        }

        public double A {
            get => (double) this.GetValue(AProperty);
            set => this.SetValue(AProperty, value);
        }

        protected override System.Windows.Media.Media3D.Point3D Project(Numerics.MemoizeMath u, Numerics.MemoizeMath v) {
            var a = this.A;

            var x = u.Cos * (a + v.Sin * u.Cos - Math.Sin(2.0 * v.Value) * u.Sin / 2.0);
            var y = u.Sin * (a + v.Sin * u.Cos - Math.Sin(2.0 * v.Value) * u.Sin / 2.0);
            var z = u.Sin * v.Sin + u.Cos * Math.Sin(2.0 * v.Value) / 2.0;
            return new System.Windows.Media.Media3D.Point3D(x, y, z);
        }
    }
}