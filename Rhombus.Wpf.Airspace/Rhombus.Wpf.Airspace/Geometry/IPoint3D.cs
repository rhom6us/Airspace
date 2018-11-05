using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A generic interface for q 3-dimensional point.
    /// </summary>
    public interface IPoint3D<T> {
        T X { get; }
        T Y { get; }
        T Z { get; }
    }
}