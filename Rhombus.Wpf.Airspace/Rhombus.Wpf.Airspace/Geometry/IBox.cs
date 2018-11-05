using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A Box is the generalization of a rectangle for higher dimensions,
    ///     formally defined as the Cartesian product of intervals.
    /// </summary>
    public interface IBox<T> {
        int Dimensions { get; }
        Numerics.Interval<T> GetInterval(int dimension);
    }
}