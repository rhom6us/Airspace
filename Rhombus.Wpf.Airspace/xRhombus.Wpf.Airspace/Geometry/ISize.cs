using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A generic interface for an n-dimensional size.
    /// </summary>
    /// <remarks>
    ///     Size is similar to Vector, except that the extents cannot be
    ///     negative.
    /// </remarks>
    public interface ISize<T> {
        int Dimensions { get; }
        T GetExtent(int dimension);
    }
}