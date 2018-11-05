using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A generic interface for a 2-dimensional point.
    /// </summary>
    public interface IPoint2D<T> {
        T X { get; }
        T Y { get; }
    }
}