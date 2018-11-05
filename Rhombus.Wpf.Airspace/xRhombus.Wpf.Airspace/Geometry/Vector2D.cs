using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A simple implementation of a 2-dimensional point.
    /// </summary>
    public struct Vector2D<T> : IVector2D<T> {
        public Vector2D(T deltaX, T deltaY) : this() {
            this.DeltaX = deltaX;
            this.DeltaY = deltaY;
        }

        public T DeltaX { get; }
        public T DeltaY { get; }
    }
}