using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A generic interface for a 3-dimensional box.
    /// </summary>
    public interface IBox3D<T> {
        Numerics.Interval<T> Width { get; }
        Numerics.Interval<T> Height { get; }
        Numerics.Interval<T> Depth { get; }
    }
}