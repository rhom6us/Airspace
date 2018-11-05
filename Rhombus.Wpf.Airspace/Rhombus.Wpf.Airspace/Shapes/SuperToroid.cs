using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Shapes {
    public class SuperToroid : ParametricShape3D {
        public static System.Windows.DependencyProperty N1Property = System.Windows.DependencyProperty.Register("N1", typeof(double), typeof(SuperToroid), new System.Windows.PropertyMetadata(2.0, Shape3D.OnPropertyChangedAffectsModel));

        public static System.Windows.DependencyProperty N2Property = System.Windows.DependencyProperty.Register("N2", typeof(double), typeof(SuperToroid), new System.Windows.PropertyMetadata(2.0, Shape3D.OnPropertyChangedAffectsModel));

        static SuperToroid() {
            // So texture coordinates work out better, configure the default
            // MinV property to be PI.
            MinVProperty.OverrideMetadata(typeof(SuperToroid), new System.Windows.PropertyMetadata(Math.PI));

            // So texture coordinates work out better, configure the default
            // MaxV property to be 3*PI.
            MaxVProperty.OverrideMetadata(typeof(SuperToroid), new System.Windows.PropertyMetadata(Math.PI * 3.0));
        }

        public double N1 {
            get => (double) this.GetValue(N1Property);
            set => this.SetValue(N1Property, value);
        }

        public double N2 {
            get => (double) this.GetValue(N2Property);
            set => this.SetValue(N2Property, value);
        }

        protected override System.Windows.Media.Media3D.Point3D Project(Numerics.MemoizeMath u, Numerics.MemoizeMath v) {
            var centerRadius = 2.0;
            var crossSectionRadius = 1.0;
            var n1 = this.N1;
            var n2 = this.N2;

            var x = (centerRadius + crossSectionRadius * SuperToroid.SafePow(v.Cos, this.N2)) * SuperToroid.SafePow(Math.Cos(-u.Value), n1);
            var y = (centerRadius + crossSectionRadius * SuperToroid.SafePow(v.Cos, this.N2)) * SuperToroid.SafePow(Math.Sin(-u.Value), n1);
            var z = crossSectionRadius * SuperToroid.SafePow(v.Sin, n2);
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