using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Shapes {
    public sealed class BoysSurface : ParametricShape3D {
        protected override System.Windows.Media.Media3D.Point3D Project(Numerics.MemoizeMath u, Numerics.MemoizeMath v) {
            var x = this.X(u, v);
            var y = this.Y(u, v);
            var z = this.Z(u, v);

            var x2 = x * x;
            var y2 = y * y;
            var z2 = z * z;

            var f = (2.0 * x2 - y2 - z2 + 2.0 * y * z * (y2 - z2) + z * x * (x2 - z2) + x * y * (y2 - x2)) / 2.0;
            var g = (y2 - z2 + (z * x * (z2 - x2) + x * y * (y2 - x2))) * Math.Sqrt(3) / 2.0;
            var h = (x + y + z) * (Math.Pow(x + y + z, 3) + 4.0 * (y - x) * (z - y) * (x - z));
            return new System.Windows.Media.Media3D.Point3D(f, g, h / 8.0);
        }

        private double X(Numerics.MemoizeMath u, Numerics.MemoizeMath v) {
            return u.Cos * v.Sin;
        }

        private double Y(Numerics.MemoizeMath u, Numerics.MemoizeMath v) {
            return u.Sin * v.Sin;
        }

        private double Z(Numerics.MemoizeMath u, Numerics.MemoizeMath v) {
            return v.Cos;
        }
    }
}