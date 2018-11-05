using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A generic interface for a 3-dimensional vector.
    /// </summary>
    public interface IVector3D<T> {
        T DeltaX { get; }
        T DeltaY { get; }
        T DeltaZ { get; }
    }
}