using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A generic interface for a 1-dimensional point.
    /// </summary>
    public interface IPoint1D<T> {
        T X { get; }
    }
}