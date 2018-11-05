using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A simple implementation of a 1-dimensional vector.
    /// </summary>
    public struct Vector1D<T> : IVector1D<T> {
        public Vector1D(T deltaX) : this() {
            this.DeltaX = deltaX;
        }

        public T DeltaX { get; }
    }
}