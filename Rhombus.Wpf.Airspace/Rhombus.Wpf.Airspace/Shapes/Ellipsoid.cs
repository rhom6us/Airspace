using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Shapes {
    public class Ellipsoid : ParametricShape3D {
        protected override System.Windows.Media.Media3D.Point3D Project(Numerics.MemoizeMath u, Numerics.MemoizeMath v) {
            var xRadius = 1.0;
            var yRadius = 1.0;
            var zRadius = 1.0;

            var x = xRadius * u.Cos * v.Sin;
            var y = yRadius * u.Sin * v.Sin;
            var z = zRadius * v.Cos;
            return new System.Windows.Media.Media3D.Point3D(x, y, z);
        }
    }
}