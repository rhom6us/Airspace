using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A generic interface for an n-dimensional vector.
    /// </summary>
    public interface IVector<T> {
        int Dimensions { get; }
        T GetDelta(int dimension);
    }
}