using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Shapes {
    public class Torus : ParametricShape3D {
        static Torus() {
            // So texture coordinates work out better, configure the default
            // MinV property to be PI.
            MinVProperty.OverrideMetadata(typeof(Torus), new System.Windows.PropertyMetadata(Math.PI));

            // So texture coordinates work out better, configure the default
            // MaxV property to be 3*PI.
            MaxVProperty.OverrideMetadata(typeof(Torus), new System.Windows.PropertyMetadata(Math.PI * 3.0));
        }

        protected override System.Windows.Media.Media3D.Point3D Project(Numerics.MemoizeMath u, Numerics.MemoizeMath v) {
            var centerRadius = 2.0;
            var crossSectionRadius = 1.0;

            var x = (centerRadius + crossSectionRadius * v.Cos) * Math.Cos(-u.Value);
            var y = (centerRadius + crossSectionRadius * v.Cos) * Math.Sin(-u.Value);
            var z = crossSectionRadius * v.Sin;
            return new System.Windows.Media.Media3D.Point3D(x, y, z);
        }
    }
}