using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Shapes {
    public sealed class Cylinder : ParametricShape3D {
        static Cylinder() {
            // The height of the cylinder is specified by MaxV, so make the
            // default MaxV property be 1.
            MaxVProperty.OverrideMetadata(typeof(Cylinder), new System.Windows.PropertyMetadata(1.0));
        }

        protected override System.Windows.Media.Media3D.Point3D Project(Numerics.MemoizeMath u, Numerics.MemoizeMath v) {
            double radius = 1;

            var x = radius * u.Sin;
            var y = radius * u.Cos;
            var z = v.Value;
            return new System.Windows.Media.Media3D.Point3D(x, y, z);
        }
    }
}