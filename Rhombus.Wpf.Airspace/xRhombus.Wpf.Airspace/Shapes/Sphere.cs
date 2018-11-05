using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Shapes {
    public sealed class Sphere : ParametricShape3D {
        protected override System.Windows.Media.Media3D.Point3D Project(Numerics.MemoizeMath u, Numerics.MemoizeMath v) {
            var radius = 1.0;

            var x = radius * u.Cos * v.Sin;
            var y = radius * u.Sin * v.Sin;
            var z = radius * v.Cos;
            return new System.Windows.Media.Media3D.Point3D(x, y, z);
        }
    }
}