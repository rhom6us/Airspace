using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A generic interface for a 3-dimensional size.
    /// </summary>
    public interface ISize3D<T> {
        T Width { get; }
        T Height { get; }
        T Depth { get; }
    }
}