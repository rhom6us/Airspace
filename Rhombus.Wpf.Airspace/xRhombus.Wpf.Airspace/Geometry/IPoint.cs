using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A generic interface for an n-dimensional point.
    /// </summary>
    public interface IPoint<T> {
        int Dimensions { get; }
        T GetCoordinate(int dimension);
    }
}