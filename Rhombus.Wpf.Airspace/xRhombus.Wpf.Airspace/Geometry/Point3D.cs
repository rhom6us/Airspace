using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A simple implementation of a 3-dimensional point.
    /// </summary>
    public struct Point3D<T> : IPoint3D<T> {
        public Point3D(T x, T y, T z) : this() {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public T X { get; }
        public T Y { get; }
        public T Z { get; }
    }
}