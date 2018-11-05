using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A simple implementation of a 3-dimensional vector.
    /// </summary>
    public struct Vector3D<T> : IVector3D<T> {
        public Vector3D(T deltaX, T deltaY, T deltaZ) : this() {
            this.DeltaX = deltaX;
            this.DeltaY = deltaY;
            this.DeltaZ = deltaZ;
        }

        public T DeltaX { get; }
        public T DeltaY { get; }
        public T DeltaZ { get; }
    }
}