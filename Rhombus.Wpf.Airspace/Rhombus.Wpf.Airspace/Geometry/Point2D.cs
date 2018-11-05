using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A simple implementation of a 2-dimensional point.
    /// </summary>
    public struct Point2D<T> : IPoint2D<T> {
        public Point2D(T x, T y) : this() {
            this.X = x;
            this.Y = y;
        }

        public T X { get; }
        public T Y { get; }
    }
}