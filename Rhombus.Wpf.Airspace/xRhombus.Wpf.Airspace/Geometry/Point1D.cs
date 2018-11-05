using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A simple implementation of a 1-dimensional point.
    /// </summary>
    public struct Point1D<T> : IPoint1D<T> {
        public Point1D(T x) : this() {
            this.X = x;
        }

        public T X { get; }
    }
}